import 'package:flutter/material.dart';

class LoginFormModel extends ChangeNotifier {
  String _email = '';
  String _password = '';
  bool _isLoading = false;

  String get email => _email;
  String get password => _password;
  bool get isLoading => _isLoading;

  void updateEmail(String value) {
    _email = value;
    notifyListeners();
  }

  void updatePassword(String value) {
    _password = value;
    notifyListeners();
  }

  void setLoading(bool value) {
    _isLoading = value;
    notifyListeners();
  }

  String? validateEmail(String? value) {
    if (value == null || value.isEmpty) return 'insira um email';
    if (!value.contains('@')) return 'email inválido';
    return null;
  }

  String? validatePassword(String? value) {
    if (value == null || value.isEmpty) return 'insira uma senha';
    if (value.length <= 6) return 'a senha deve ter mais de 6 caracteres';
    return null;
  }

  Future<bool> submitLogin(GlobalKey<FormState> formKey) async {
    if (!formKey.currentState!.validate()) return false;

    setLoading(true);
    try {
      await Future.delayed(const Duration(seconds: 2));
      return true;
    } catch (e) {
      debugPrint('Erro ao fazer login: $e');
      return false;
    } finally {
      setLoading(false);
    }
  }
}
