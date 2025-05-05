class UserModel {
  final String email;

  const UserModel({required this.email});

  UserModel copyWith({String? email}) {
    return UserModel(
      email: email ?? this.email,
    );
  }

  @override
  String toString() => 'UserModel(email: $email)';
}
