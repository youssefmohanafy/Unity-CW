# Unity-CW
# PowderCoat QC

PowderCoat QC is a cross-platform mobile application built with Unity 6.3 for Android and iOS. The app is designed for factory environments that use powder coating processes. It provides a digital solution for quality control inspectors and supervisors to report, manage, and review powder coating defects.

Instead of using paper forms or verbal communication, workers can open the app, fill in a defect report, select the defect type, severity, and root cause, and optionally attach a photo. Managers can then log in and review all submitted reports, marking them as done when corrective action has been taken. All data is stored in Firebase Firestore and is accessible in real time by all authorised users.

## Features

- Firebase Authentication login with role-based access (worker / manager)
- Submit defect reports with type, severity, root cause, and photo
- View defect history (workers see their own reports, managers see all)
- Manager review screen to mark reports as done
- Real-time data storage using Firebase Firestore

## Third-Party Plugins

| Plugin | Purpose |
|--------|---------|
| Firebase Unity SDK | User authentication and Firestore database |
| NativeGallery (yasirkula) | Native photo picker for iOS and Android |
| TextMeshPro | High-quality UI text rendering (built into Unity) |

## Steps to Run the Project

1. Clone this repository to your local machine
2. Open **Unity Hub** and click **Open Project**, then select the cloned folder
3. Make sure you are using **Unity 6.3 LTS** with iOS and Android build support installed
4. Open **File > Build Profiles** and ensure all 6 scenes are listed in order: Login, Dashboard, NewDefect, DefectHistory, DefectDetails, ManagerReview
5. Press the **Play** button in Unity to run the app in the editor using the Device Simulator
6. To build for a real device, go to **File > Build Profiles**, select iOS or Android, and click **Build**

## Test Accounts

- **Worker:** worker@test.com / 123456
- **Manager:** manager@test.com / 123456

## Project Structure

```
Assets/
  Scenes/       — All 6 app screens
  Scripts/      — Manager scripts for each scene
  Prefabs/      — DefectCard prefab for the defect list
  Plugins/      — NativeGallery, Firebase SDK
```
