import 'package:firebase_core/firebase_core.dart';
import 'package:flutter/foundation.dart';

Future initFirebase() async {
  if (kIsWeb) {
    await Firebase.initializeApp(
        options: FirebaseOptions(
            apiKey: "AIzaSyA7qDH1SVA4PwcJ3a2DdvTYPF9-lAYKCVg",
            authDomain: "investment-tracker-79835.firebaseapp.com",
            projectId: "investment-tracker-79835",
            storageBucket: "investment-tracker-79835.firebasestorage.app",
            messagingSenderId: "407297455198",
            appId: "1:407297455198:web:3f11ac162d21fe7f7970cf",
            measurementId: "G-W6DKN2ER1K"));
  } else {
    await Firebase.initializeApp();
  }
}
