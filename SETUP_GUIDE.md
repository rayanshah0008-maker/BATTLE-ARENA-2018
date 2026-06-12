# 🎮 BATTLE ARENA 2018 - Complete Setup Guide

## ⚡ Quick Start (5 Minutes)

### Step 1: Install Unity
1. Download Unity 2021 LTS: https://unity.com/download
2. Install with Android Build Support
3. Open Unity Hub

### Step 2: Clone Repository
```bash
git clone https://github.com/rayanshah0008-maker/BATTLE-ARENA-2018.git
cd BATTLE-ARENA-2018
```

### Step 3: Open Project
1. Unity Hub > Open > Select BATTLE-ARENA-2018 folder
2. Wait for import to complete (~5 minutes)
3. Open Scenes > MainMenu.unity

### Step 4: Setup Photon (Multiplayer)
1. Go: https://www.photonengine.com/
2. Sign up (FREE)
3. Create new app > Copy App ID
4. In Unity: Window > Photon > PUN 2 > Settings
5. Paste App ID in "App ID (Photon)"

### Step 5: Test Game
1. Press Play button in Unity Editor
2. Enter player name
3. Click "Play"
4. Should connect to Photon (check Console)

---

## 📱 Building APK (Android)

### Prerequisites
- Android SDK (Android Studio or SDK Manager)
- Java Development Kit (JDK)
- USB cable for testing

### Steps

#### 1. Setup Android Build
```
File > Build Settings > Android > Switch Platform
```

#### 2. Configure Player Settings
```
Edit > Project Settings > Player:
- Company Name: Your Name
- Product Name: BATTLE ARENA 2018
- Minimum API Level: Android 6.0 (API 23)
- Target API Level: Android 13 (API 33)
- Screen Orientation: Landscape
```

#### 3. Build APK
```
File > Build Settings
Click "Build"
Choose folder: builds/android
Wait for build to complete (~10 minutes)
```

#### 4. Test on Device
```bash
# Enable Developer Mode on phone:
# Settings > About > Tap Build Number 7 times
# Settings > Developer Options > Enable USB Debugging

# Connect phone via USB cable
# In folder with APK:
adb install BATTLE-ARENA-2018.apk

# Or drag APK to phone file manager
```

---

## 🎮 Gameplay Guide

### Game Modes
- **Solo**: 50 players, last one wins
- **Duo**: 25 teams of 2
- **Squad**: 12 teams of 4

### Controls (Android)
```
Left Joystick   - Move
Right Joystick  - Look Around
Red Button      - Shoot
Green Button    - Reload
Blue Button     - Jump
Yellow Button   - Inventory
```

### Controls (PC Testing)
```
W/A/S/D         - Move
Mouse           - Look Around
Left Click      - Shoot
Space           - Reload
C               - Crouch
X               - Prone
Tab             - Inventory
```

### Gameplay Loop
1. **Drop** from plane
2. **Land** and loot weapons
3. **Fight** other players
4. **Survive** until last player/team wins

---

## 🔧 Troubleshooting

### Issue: "Cannot connect to Photon"
**Solution**:
1. Check internet connection
2. Verify Photon App ID is correct
3. Restart Unity
4. Check Photon server status: https://status.photonengine.com/

### Issue: "Build fails - Android SDK not found"
**Solution**:
1. Go: Edit > Preferences > External Tools
2. Set Android SDK Path
3. Or install Android Studio with SDK

### Issue: "Low FPS / Performance Issues"
**Solution**:
1. Edit > Project Settings > Quality
2. Lower "Shadows: No Shadows"
3. Lower "Particle System"
4. Edit > Preferences > Graphics > Reduced resolution

### Issue: "Game crashes on spawn"
**Solution**:
1. Check Scene > Main > Spawn points exist
2. Verify Player Prefab is in Assets > Prefabs
3. Check Console for errors

---

## 📚 Project Structure Explained

```
Assets/
├── Scripts/          → All C# code
│   ├── Player/       → Movement, Health, Inventory
│   ├── Weapons/      → Weapon system, shooting
│   ├── UI/           → Menus, HUD, Settings
│   ├── Gameplay/     → Game logic, Match system
│   └── Network/      → Photon networking
├── Scenes/           → Game scenes
│   ├── MainMenu      → Start screen
│   ├── Lobby         → Room selection
│   └── GameScene     → Main gameplay
├── Prefabs/          → Reusable objects
├── Models/           → 3D character/weapon models
├── Textures/         → Texture files
└── Audio/            → Music and SFX
```

---

## 🎨 Customizing the Game

### Change Player Speed
```csharp
// In Assets/Scripts/Player/PlayerMovement.cs
public float moveSpeed = 5f;        // Change this
public float sprintSpeed = 10f;     // Or this
```

### Change Weapon Damage
```csharp
// In Assets/Scripts/Weapons/WeaponSystem.cs
// Find "InitializeWeapons()" function
weapons[0].damage = 25;  // M4 damage
```

### Change Map Size
```csharp
// In Assets/Scripts/Gameplay/GameManager.cs
public float initialSafeZoneRadius = 2000f;  // Change this
```

### Change Player Health
```csharp
// In Assets/Scripts/Player/PlayerHealth.cs
public float maxHealth = 100f;    // Change this
public float maxArmor = 100f;     // Or this
```

---

## 🌐 Networking Basics

### Photon Concepts
- **Room**: Game session (up to 50 players)
- **Player**: Individual person connected
- **RPC**: Remote Procedure Call (send data to other players)
- **PhotonView**: Component that syncs object across network

### Using RPC (Send Message to Other Players)
```csharp
// Send to all players
photonView.RPC("FunctionName", RpcTarget.All, parameter);

// Send to others only
photonView.RPC("FunctionName", RpcTarget.Others, parameter);

// Send to master client only
photonView.RPC("FunctionName", RpcTarget.MasterClient, parameter);
```

---

## 📊 Performance Tips

1. **Use Object Pooling**: Reuse bullets instead of instantiating new ones
2. **Optimize Networking**: Only send important data
3. **Reduce Draw Calls**: Use texture atlases
4. **LOD System**: Lower detail models at distance
5. **Async Loading**: Load assets in background

---

## 🚀 Publishing to Play Store

### Before Publishing
- ✅ Test thoroughly on real devices
- ✅ Get age rating (IARC)
- ✅ Create privacy policy
- ✅ Add app icon and screenshots
- ✅ Create app description

### Steps
1. Create Google Play Developer account ($25 one-time)
2. Create signed APK (Build > Build and Run)
3. Upload to Google Play Console
4. Set pricing and distribution
5. Submit for review (24-48 hours)

---

## 💡 Next Steps

1. **Try the game** in Unity Editor
2. **Build APK** and test on phone
3. **Customize** weapons/maps
4. **Invite friends** and play multiplayer
5. **Add features** (skins, cosmetics, etc.)

---

## 📞 Need Help?

- **GitHub Issues**: Report bugs on GitHub
- **Email**: rayanshahfreefire@gmail.com
- **Discord**: Join game dev community
- **Unity Forums**: Ask Unity questions

---

**Happy Gaming! 🎮** 🚀
