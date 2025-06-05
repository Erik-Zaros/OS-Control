import 'package:cloud_firestore/cloud_firestore.dart'; // Importe o Firestore
import 'package:firebase_auth/firebase_auth.dart';

class Task {
  String id;
  String title;
  String description;
  bool isCompleted;
  bool isFavorite;
  DateTime createdAt;

  Task({
    required this.id,
    required this.title,
    this.description = '',
    this.isCompleted = false,
    this.isFavorite = false,
    required this.createdAt,
  });

  factory Task.createNew({
    required String title,
    String description = '',
    bool isCompleted = false,
    bool isFavorite = false,
  }) {
    return Task(
      id: '', 
      title: title,
      description: description,
      isCompleted: isCompleted,
      isFavorite: isFavorite,
      createdAt: DateTime.now(),
    );
  }


  Map<String, dynamic> toJson() {
    return {
      'title': title,
      'description': description,
      'isCompleted': isCompleted,
      'isFavorite': isFavorite,
      'createdAt': Timestamp.fromDate(createdAt),
      'userId': FirebaseAuth.instance.currentUser?.uid, 
    };
  }

  factory Task.fromFirestore(DocumentSnapshot<Map<String, dynamic>> snapshot, SnapshotOptions? options) {
    final data = snapshot.data();
    if (data == null) {
      throw StateError('missing data for task ${snapshot.id}');
    }
    return Task(
      id: snapshot.id, 
      title: data['title'] as String? ?? '',
      description: data['description'] as String? ?? '',
      isCompleted: data['isCompleted'] as bool? ?? false,
      isFavorite: data['isFavorite'] as bool? ?? false,
      createdAt: (data['createdAt'] as Timestamp?)?.toDate() ?? DateTime.now(),
    );
  }

  Task copyWith({
    String? id,
    String? title,
    String? description,
    bool? isCompleted,
    bool? isFavorite,
    DateTime? createdAt,
  }) {
    return Task(
      id: id ?? this.id,
      title: title ?? this.title,
      description: description ?? this.description,
      isCompleted: isCompleted ?? this.isCompleted,
      isFavorite: isFavorite ?? this.isFavorite,
      createdAt: createdAt ?? this.createdAt,
    );
  }
}