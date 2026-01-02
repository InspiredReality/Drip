# DROP - Firebase Setup Guide (10 Minutes)

## 🔥 Step 1: Create Firebase Project (3 minutes)

1. **Go to Firebase Console:**
   - Visit: https://console.firebase.google.com
   - Click "Add project"

2. **Create Project:**
   - Project name: "drop-game"
   - Disable Google Analytics (not needed for MVP)
   - Click "Create project"
   - Wait ~30 seconds
   - Click "Continue"

## 📊 Step 2: Set Up Realtime Database (2 minutes)

1. **In Firebase Console:**
   - Click "Realtime Database" in left menu
   - Click "Create Database"
   
2. **Choose Location:**
   - Select closest region (e.g., us-central1)
   - Click "Next"

3. **Security Rules:**
   - Select "Start in **test mode**" (for development)
   - Click "Enable"

4. **IMPORTANT - Update Rules:**
   - Go to "Rules" tab
   - Replace with this:

```json
{
  "rules": {
    "games": {
      "$gameId": {
        ".read": true,
        ".write": true,
        ".indexOn": ["code", "createdAt"]
      }
    }
  }
}
```

   - Click "Publish"

## 🔑 Step 3: Get Firebase Config (2 minutes)

1. **Add Web App:**
   - Click gear icon (⚙️) → "Project settings"
   - Scroll to "Your apps"
   - Click "</>" (Web icon)
   - App nickname: "Drop Web"
   - **Don't** check "Firebase Hosting"
   - Click "Register app"

2. **Copy Config:**
   - You'll see something like this:

```javascript
const firebaseConfig = {
  apiKey: "AIzaSyA...",
  authDomain: "drop-game-xxxxx.firebaseapp.com",
  databaseURL: "https://drop-game-xxxxx.firebaseio.com",
  projectId: "drop-game-xxxxx",
  storageBucket: "drop-game-xxxxx.appspot.com",
  messagingSenderId: "123456789",
  appId: "1:123456789:web:abcdef"
};
```

   - **COPY THIS!**

## 📝 Step 4: Update Your HTML File (1 minute)

1. **Open `drop-online.html`**

2. **Find this section (around line 290):**

```javascript
const firebaseConfig = {
    apiKey: "YOUR_API_KEY",
    authDomain: "YOUR_PROJECT.firebaseapp.com",
    // ...
};
```

3. **Replace with YOUR config** (the one you copied)

4. **Save the file**

## 🚀 Step 5: Deploy (2 minutes)

### Option A: GitHub Pages

1. **Rename file:**
   - `drop-online.html` → `index.html`

2. **Create repo and upload:**
   - `index.html`
   - `manifest.json`
   - `service-worker.js`
   - `icons/` folder

3. **Enable Pages:**
   - Settings → Pages → Source: main branch
   - Done! Game is live at `https://username.github.io/drop-game`

### Option B: Firebase Hosting (Bonus!)

```bash
# Install Firebase CLI
npm install -g firebase-tools

# Login
firebase login

# Initialize
firebase init hosting
# Select your project
# Public directory: .
# Single-page app: Yes
# Overwrites: No

# Deploy
firebase deploy --only hosting

# Your game is live at: https://drop-game-xxxxx.web.app
```

## ✅ Step 6: Test It Works!

1. **Open your deployed site**

2. **Create a game:**
   - Enter name: "Player 1"
   - Click "Create New Game"
   - You'll see a 6-digit code (e.g., "ABC123")

3. **Open in another tab/phone:**
   - Enter name: "Player 2"
   - Click "Join Game"
   - Enter the code
   - Both players should see the game start!

## 🎮 How It Works

**Game Flow:**
```
Player 1                    Firebase                    Player 2
--------                    --------                    --------
Create game          →      Store in DB
                            Generate code
                            
Show waiting screen
                            
                     ←      Listen for changes   ←      Join game with code
                            
                            Update: 2 players
                            
Game starts!                                             Game starts!
```

**Turn-Based Sync:**
```
Player 1's Turn:
1. Select crown         → Update DB
2. Play drop phase      → Update balls count
3. Shoot balloons       → Update opponent HP
4. End turn             → Switch to Player 2

Player 2 sees:
- "Waiting..." during P1's drop phase
- Updated balloon HP after P1 shoots
- "Your turn!" notification
```

## 📊 Firebase Database Structure

```
games/
  {gameId}/
    code: "ABC123"
    state: "crown_select" | "drop" | "shoot" | "game_over"
    currentPlayer: "user_123..."
    turn: 0
    createdAt: 1234567890
    players/
      {userId1}/
        name: "Player 1"
        balls: 7
        balloons: [5, 3, 5]
        crown: 1
        isCreator: true
      {userId2}/
        name: "Player 2"
        balls: 0
        balloons: [5, 5, 4]
        crown: 0
        isCreator: false
```

## 🔒 Security Notes

**Current Setup (Test Mode):**
- ⚠️ Anyone can read/write data
- ⚠️ Only use for MVP/testing
- ⚠️ Data can be deleted by anyone

**For Production (Add Later):**

```json
{
  "rules": {
    "games": {
      "$gameId": {
        ".read": "auth != null",
        ".write": "auth != null && (
          !data.exists() || 
          data.child('players').child(auth.uid).exists()
        )"
      }
    }
  }
}
```

Then add Firebase Authentication:
1. Enable Email/Password auth
2. Add login screen
3. Update rules

## 💰 Cost

**Firebase Free Tier (Spark Plan):**
- ✅ 1 GB stored data (plenty for thousands of games)
- ✅ 10 GB/month downloaded (good for ~1000 active games)
- ✅ 100 concurrent connections

**For Drop MVP:**
- Expected cost: **$0/month** (unless you go viral!)
- Each game = ~5 KB
- Can handle 200,000+ games on free tier

## 🐛 Troubleshooting

**Error: "Permission denied"**
- Check Firebase rules are in test mode
- Make sure databaseURL is correct

**Players not syncing:**
- Check both players are in same game
- Open browser console (F12) to see errors
- Verify Firebase config is correct

**Game code not working:**
- Code is case-sensitive
- Must be 6 characters
- Check game hasn't expired (24hr limit in code)

## 🚀 You're Done!

You now have:
- ✅ Online multiplayer working
- ✅ Async turn-based gameplay
- ✅ Game codes for matchmaking
- ✅ Real-time sync via Firebase
- ✅ Free hosting

Share your game code with friends and play! 🎉

---

**Total setup time:** ~10 minutes  
**Total cost:** $0  
**Players supported:** Unlimited (within free tier)