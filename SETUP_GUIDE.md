# 🛠️ BATTLE ARENA 2018 - Setup Guide

## Step 1: Prerequisites

### Software Required
- **Unity 2022 LTS or higher** (Free version)
- **Android SDK** (Latest)
- **Git** (Version control)
- **Visual Studio Code** or **Visual Studio** (Code editor)

### Hardware Requirements
- **CPU:** Intel i5 or equivalent
- **RAM:** 8GB minimum
- **Disk Space:** 20GB for Unity + project
- **Android Device:** For testing (optional, can use emulator)

## Step 2: Install Unity

1. Download from https://unity.com/download
2. Install Unity Hub
3. In Hub, install Unity 2022 LTS
4. Size: ~3-4 GB (wait for complete installation)

## Step 3: Clone Repository

```bash
git clone https://github.com/rayanshah0008-maker/BATTLE-ARENA-2018.git
cd BATTLE-ARENA-2018
```

## Step 4: Open in Unity

1. Open Unity Hub
2. Click "Add project from disk"
3. Select the BATTLE-ARENA-2018 folder
4. Open with Unity 2022 LTS
5. Wait for initial import (~5-10 minutes)

## Step 5: Import Photon PUN 2

1. In Unity, go to **Window → TextMesh Pro → Import TMP Essential Resources**
2. Go to **Asset Store** (in Unity)
3. Search: "Photon PUN 2"
4. Download and Import
5. Follow Photon setup wizard
6. Create free Photon account
7. Enter App ID in PhotonNetwork settings

## Step 6: Android Build Setup

### Edit → Project Settings → Player

**Graphics Settings:**
- Rendering Path: Forward
- Color Space: Linear (recommended)

**Resolution:**
- Width: 1920
- Height: 1080
- Orientation: Landscape Left and Right

**Other Settings:**
- Min API Level: 24 (Android 7.0)
- Target API Level: 33 (Android 13)
- Package Name: com.arena.battleroyale

## Step 7: Create Scenes

### Scene 1: MainMenu
- File → New Scene
- Name: MainMenu
- Add UI Canvas
- Add buttons: Play, Settings, Quit

### Scene 2: Lobby
- Duplicate MainMenu scene
- Name: Lobby
- Add mode selection buttons (Solo, Duo, Squad)

### Scene 3: GameScene
- Create new scene
- Name: GameScene
- Add 3D objects: Terrain, Buildings, Spawn points

## Step 8: Build APK

1. **File → Build Settings**
2. **Platform: Android**
3. **Click "Switch Platform"** (wait for import)
4. **Add scenes to build:**
   - MainMenu
   - Lobby
   - GameScene
5. **Build APK:**
   - File → Build And Run
   - Choose output folder
   - Name: BATTLE-ARENA-2018.apk
   - Wait for build completion (~5-10 minutes)

## Step 9: Test on Device

### Option A: Physical Device
1. Connect Android phone via USB
2. Enable Developer Mode (Settings → About → Tap Build Number 7 times)
3. Enable USB Debugging
4. Click "Build and Run" in Unity
5. APK installs automatically

### Option B: Android Emulator
1. Install Android Studio
2. Create Android Virtual Device (AVD)
3. Launch emulator
4. Click "Build and Run" in Unity

## Step 10: Verify Installation

✅ Game launches successfully
✅ Main menu appears
✅ Can select game modes
✅ Can see HUD in game
✅ Controls respond to input
✅ No errors in console

## Troubleshooting

### Problem: Photon not connecting
**Solution:**
- Check internet connection
- Verify App ID in Photon settings
- Check if Photon servers are running

### Problem: Build fails
**Solution:**
- Update Android SDK
- Clear Unity cache: Assets → Reimport All
- Restart Unity

### Problem: Low FPS
**Solution:**
- Reduce texture quality
- Disable shadows
- Use LOD groups

### Problem: APK won't install
**Solution:**
- Enable Unknown Sources on device
- Check device storage space
- Try installing on different device

## Next Steps

1. ✅ Setup complete
2. 📝 Start modifying scripts
3. 🎨 Add 3D models
4. 🧪 Test gameplay
5. 🚀 Deploy APK

## Resources

- **Unity Docs:** https://docs.unity.com
- **Photon Docs:** https://doc.photonengine.com
- **Android Dev:** https://developer.android.com

---

**Setup Complete! Ready to develop!** 🚀