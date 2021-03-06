/*
 * File: JustAnotherVoiceChat.js
 * Date: 22.2.2018,
 *
 * MIT License
 *
 * Copyright (c) 2018 JustAnotherVoiceChat
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

let voiceHandler = null;
let rotationThreshold = 0.001;

API.onResourceStart.connect(() => { voiceHandler = new GtmpVoiceHandler(); });

API.onResourceStop.connect(() => { 
    voiceHandler.dispose(); 
    voiceHandler = null;
});

API.onServerEventTrigger.connect((eventName, args) => {
    switch (eventName) {
        case "VOICE_SET_HANDSHAKE": {
            if (args.Length === 2) {
                voiceHandler.setHandshake(args[0], args[1]);
            } else {
                voiceHandler.setHandshake(args[0], "");
            }
            break;
        }
    }
});

API.onUpdate.connect(() => {
    const players = API.getStreamedPlayers();
    const localPosition = API.getEntityPosition(API.getLocalPlayer());
    
    for(let i = 0; i < players.Length; i++) {
        const player = players[i];
        const playerPosition = API.getEntityPosition(player);
 
        const screen = API.worldToScreenMaintainRatio(playerPosition);
        
        if (screen.X > 0 && screen.Y > 0) {
            API.drawText((Math.round(localPosition.DistanceTo(playerPosition) * 10) / 10) + "m", screen.X, screen.Y, 0.5, 255, 255, 255, 255, 0, 1, false, true, 100);
        }
    }
});

class GtmpVoiceHandler {

    constructor() {
        this.handshakeTimer = -1;
        this.rotationTimer = -1;
        this.lastRotation = 0;

        this.buildBrowser();
    }

    buildBrowser() {
        this.browser = API.createCefBrowser(0, 0, false);
        API.waitUntilCefBrowserInit(this.browser);
        API.setCefBrowserHeadless(this.browser, true);
    }

    sendHandshake(url) {
        API.sendChatMessage("HS: " + url);

        API.loadPageCefBrowser(this.browser, url);
    }

    sendRotation() {
        const rotation = ((API.getGameplayCamRot().Z * -1) * Math.PI) / 180;

        if (Math.abs(this.lastRotation - rotation) < rotationThreshold) {
            return;
        }

        API.triggerServerEvent("VOICE_ROTATION", rotation);

        this.lastRotation = rotation;
    }

    setHandshake(status, url) {
        if (status === (this.handshakeTimer !== -1)) {
            return;
        }
        
        if (status) {
            if (this.rotationTimer !== -1) {
                API.stop(this.rotationTimer);
                this.rotationTimer = -1;
            }

            this.handshakeTimer = API.every(3000, "resendHandshake", url);
            resendHandshake(url);
        } else {
            if (this.handshakeTimer !== -1) {
                API.stop(this.handshakeTimer);
                this.handshakeTimer = -1;
            }

            this.rotationTimer = API.every(333, "sendCamrotation");
        }
    }

    dispose() {
        if (this.handshakeTimer !== -1) {
            API.stop(this.handshakeTimer);
            this.handshakeTimer = -1;
        }

        if (this.rotationTimer !== -1) {
            API.stop(this.rotationTimer);
            this.rotationTimer = -1;
        }
    }

}

function resendHandshake(url) {
    if (voiceHandler === null) {
        return;
    }
    
    voiceHandler.sendHandshake(url);
}

function sendCamrotation() {
    if (voiceHandler === null) {
        return;
    }
    
    voiceHandler.sendRotation();
}
