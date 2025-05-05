import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:provider/provider.dart';
import '../models/login_form_model.dart';

class LoginScreen extends StatelessWidget {
  const LoginScreen({super.key});

  @override
  Widget build(BuildContext context) {
    const appTitle = 'Bem Vindo!';
    return Scaffold(
      appBar: AppBar(title: const Text(appTitle)),
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
    final model = Provider.of<LoginFormModel>(context);

    return Form(
      key: _formKey,
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            TextFormField(
              decoration: const InputDecoration(
                labelText: 'Email',
                border: OutlineInputBorder(),
              ),
              onChanged: (value) => model.email = value,
              validator: model.validateEmail,
            ),
            const SizedBox(height: 16.0),
            TextFormField(
              obscureText: true,
              decoration: const InputDecoration(
                labelText: 'Senha',
                border: OutlineInputBorder(),
              ),
              onChanged: (value) => model.password = value,
              validator: model.validatePassword,
            ),
            Padding(
              padding: const EdgeInsets.only(top: 24.0),
              child: SizedBox(
                width: double.infinity,
                child: ElevatedButton(
                  onPressed: model.isLoading
                      ? null
                      : () async {
                          if (_formKey.currentState!.validate()) {
                            model.isLoading = true;
                            await Future.delayed(const Duration(seconds: 2));
                            model.isLoading = false;

                            if (context.mounted) {
                              context.go('/home');
                            }
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
              ),
            ),
          ],
        ),
      ),
    );
  }
}
