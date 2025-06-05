import 'package:flutter/material.dart';
import 'package:firebase_core/firebase_core.dart';
import 'package:teste_flutter_moacir/firebase_options.dart';
import 'routes/app_router.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();

  await Firebase.initializeApp(
    options: DefaultFirebaseOptions.currentPlatform,
  );

  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp.router(
      debugShowCheckedModeBanner: false,
      title: 'Meu App de Tarefas',
      routerConfig: appRouter,
    );
  }
}