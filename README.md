# PowderCoat QC

**Student:** Youssef Mohamed Hanafy  
**Student ID:** YH202000009  
**Module:** KH6004CMD — Mobile Application Development  
**University:** The Knowledge Hub Universities (Coventry University)

---

## Description

PowderCoat QC is a mobile quality control application built with Unity for factory environments. The app allows factory workers to report powder coating defects and enables managers to review and sign off on those reports. The app supports two user roles — **worker** and **manager** — with role-based UI that is automatically assigned upon first login via Firebase Authentication.

Workers can submit defect reports including defect type, root cause, severity, and an optional photo. Managers can view all submitted defects and mark them as reviewed directly from the Defect History screen.

---

## Features

- Firebase Authentication — email/password login
- Role-based access control (worker / manager)
- Submit defect reports with type, root cause, severity, and photo
- View defect history loaded from Firestore
- Manager review panel to mark defects as done
- Cloud data persistence using Firebase Firestore
- Photo upload using device gallery

---

## Third Party Plugins

- **Firebase Unity SDK** — Authentication and Cloud Firestore (firebase.google.com)
- **NativeGallery** — Photo selection from device gallery (github.com/yasirkula/UnityNativeGallery)
- **UnityMainThreadDispatcher** — Dispatching callbacks to the Unity main thread (github.com/PimDeWitte/UnityMainThreadDispatcher)
- **TextMesh Pro** — Advanced text rendering (included with Unity)

---

## Steps to Run the Project

1. Clone the repository from GitHub
2. Open the project in **Unity 6.3.10f1 (LTS)** or later
3. Make sure all Firebase plugin files are fully downloaded (if using iCloud, toggle visibility with `Cmd + Shift + .` in Finder and force download)
4. Open **File → Build Settings** and ensure the following scenes are in order:
   - Login
   - Dashboard
   - NewDefect
   - DefectHistory
5. Set the platform to **iOS** or **Android**
6. Add a valid `google-services.json` (Android) or `GoogleService-Info.plist` (iOS) from your Firebase project to the `Assets` folder
7. Press **Play** in the Unity Editor to run in the simulator, or build to a device
8. Log in using credentials created manually in the Firebase Authentication console

---

## Notes

- User roles are assigned automatically on first login (`worker` by default)
- To promote a user to `manager`, update their document in **Firestore → users → {userId} → role** to `"manager"`
- The app was developed and tested using the **Apple iPhone 13 Pro Max** simulator in Unity
