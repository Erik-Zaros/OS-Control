import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:provider/provider.dart';
import '../models/login_form_model.dart';

class LoginScreen extends StatelessWidget {
  const LoginScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Bem Vindo!')),
      body: const MyCustomForm(),
    );
  }
}

class MyCustomForm extends StatefulWidget {
  const MyCustomForm({super.key});

  @override
  MyCustomFormState createState() => MyCustomFormState();
}

class MyCustomFormState extends State<MyCustomForm> {
  final _formKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    return Form(
      key: _formKey,
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Selector<LoginFormModel, String>(
              selector: (_, model) => model.email,
              builder: (_, __, ____) {
                return TextFormField(
                  decoration: const InputDecoration(
                    labelText: 'Email',
                    border: OutlineInputBorder(),
                  ),
                  onChanged: (value) =>
                      context.read<LoginFormModel>().updateEmail(value),
                  validator: context.read<LoginFormModel>().validateEmail,
                );
              },
            ),
            const SizedBox(height: 16.0),
            Selector<LoginFormModel, String>(
              selector: (_, model) => model.password,
              builder: (_, __, ____) {
                return TextFormField(
                  obscureText: true,
                  decoration: const InputDecoration(
                    labelText: 'Senha',
                    border: OutlineInputBorder(),
                  ),
                  onChanged: (value) =>
                      context.read<LoginFormModel>().updatePassword(value),
                  validator: context.read<LoginFormModel>().validatePassword,
                );
              },
            ),
            const SizedBox(height: 24),
            Consumer<LoginFormModel>(
              builder: (_, model, __) {
                return SizedBox(
                  width: double.infinity,
                  child: ElevatedButton(
                    onPressed: model.isLoading
                      ? null
                      : () async {
                          final success = await model.submitLogin(_formKey);
                          if (success && context.mounted) {
                            context.go('/home');
                          }
                        },

                    child: Padding(
                      padding: const EdgeInsets.symmetric(vertical: 12.0),
                      child: model.isLoading
                          ? const SizedBox(
                              height: 20,
                              width: 20,
                              child: CircularProgressIndicator(
                                strokeWidth: 2,
                                color: Colors.white,
                              ),
                            )
                          : const Text('Login'),
                    ),
                  ),
                );
              },
            ),
          ],
        ),
      ),
    );
  }
}
