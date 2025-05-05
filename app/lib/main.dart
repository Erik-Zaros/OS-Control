import 'package:flutter/material.dart';
import 'routes/app_router.dart';
import 'package:provider/provider.dart';
import 'package:teste_flutter_moacir/models/login_form_model.dart';

void main() => runApp(
  ChangeNotifierProvider(
    create: (_) => LoginFormModel(),
    child: const MyApp(),
  ),
);

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp.router(
      debugShowCheckedModeBanner: false,
      title: 'Meu App',
      routerConfig: appRouter,
    );
  }
}