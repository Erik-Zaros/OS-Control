import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:firebase_auth/firebase_auth.dart';
import '../models/task.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  final TextEditingController _taskTitleController = TextEditingController();
  final TextEditingController _taskDescriptionController = TextEditingController();
  final TextEditingController _editTaskTitleController = TextEditingController();
  final TextEditingController _editTaskDescriptionController = TextEditingController();

  late CollectionReference<Task> _tasksCollection;
  User? _currentUser; 

  @override
  void initState() {
    super.initState();
    _currentUser = FirebaseAuth.instance.currentUser; 
    if (_currentUser == null) {
      WidgetsBinding.instance.addPostFrameCallback((_) {
        context.go('/');
      });
    } else {
      _tasksCollection = FirebaseFirestore.instance
          .collection('users')
          .doc(_currentUser!.uid) 
          .collection('tasks')
          .withConverter<Task>(
            fromFirestore: Task.fromFirestore,
            toFirestore: (Task task, _) => task.toJson(),
          );
    }
  }

  @override
  void dispose() {
    _taskTitleController.dispose();
    _taskDescriptionController.dispose();
    _editTaskTitleController.dispose();
    _editTaskDescriptionController.dispose();
    super.dispose();
  }

  Future<void> _addTask() async {
    showDialog(
      context: context,
      builder: (context) {
        _taskTitleController.clear();
        _taskDescriptionController.clear();
        return AlertDialog(
          title: const Text('Adicionar Nova Tarefa'),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              TextField(
                controller: _taskTitleController,
                decoration: const InputDecoration(labelText: 'Título da Tarefa'),
              ),
              TextField(
                controller: _taskDescriptionController,
                decoration: const InputDecoration(labelText: 'Descrição (Opcional)'),
                maxLines: 3,
              ),
            ],
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: const Text('Cancelar'),
            ),
            ElevatedButton(
              onPressed: () async {
                if (_taskTitleController.text.isNotEmpty) {
                  final newTask = Task.createNew(
                    title: _taskTitleController.text,
                    description: _taskDescriptionController.text,
                  );
                  await _tasksCollection.add(newTask); 
                  if (mounted) Navigator.of(context).pop();
                }
              },
              child: const Text('Adicionar'),
            ),
          ],
        );
      },
    );
  }

  Future<void> _editTask(Task task) async {
    _editTaskTitleController.text = task.title;
    _editTaskDescriptionController.text = task.description;

    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text('Editar Tarefa'),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              TextField(
                controller: _editTaskTitleController,
                decoration: const InputDecoration(labelText: 'Título da Tarefa'),
              ),
              TextField(
                controller: _editTaskDescriptionController,
                decoration: const InputDecoration(labelText: 'Descrição (Opcional)'),
                maxLines: 3,
              ),
            ],
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: const Text('Cancelar'),
            ),
            ElevatedButton(
              onPressed: () async {
                if (_editTaskTitleController.text.isNotEmpty) {
                  await _tasksCollection.doc(task.id).update({ 
                    'title': _editTaskTitleController.text,
                    'description': _editTaskDescriptionController.text,
                  });
                  if (mounted) Navigator.of(context).pop();
                }
              },
              child: const Text('Salvar'),
            ),
          ],
        );
      },
    );
  }

  Future<void> _toggleComplete(Task task) async {
    await _tasksCollection.doc(task.id).update({
      'isCompleted': !task.isCompleted,
    });
  }

  Future<void> _toggleFavorite(Task task) async {
    await _tasksCollection.doc(task.id).update({
      'isFavorite': !task.isFavorite,
    });
  }

  Future<void> _deleteTask(Task task) async {
    await _tasksCollection.doc(task.id).delete();
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Tarefa "${task.title}" removida.'),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    if (_currentUser == null) {
      return const Scaffold(
        body: Center(child: CircularProgressIndicator()),
      );
    }

    return Scaffold(
      appBar: AppBar(
        title: const Text('Minhas Tarefas'),
        actions: [
          IconButton(
            icon: const Icon(Icons.logout),
            onPressed: () async {
              await FirebaseAuth.instance.signOut();
              if (mounted) {
                context.go('/');
              }
            },
          ),
        ],
      ),
      body: StreamBuilder<QuerySnapshot<Task>>(
        stream: _tasksCollection.orderBy('createdAt', descending: true).snapshots(),
        builder: (context, snapshot) {
          if (snapshot.hasError) {
            return Center(child: Text('Erro ao carregar tarefas: ${snapshot.error}'));
          }

          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          }

          if (!snapshot.hasData || snapshot.data!.docs.isEmpty) {
            return const Center(child: Text('Nenhuma tarefa ainda. Adicione uma!'));
          }

          final tasks = snapshot.data!.docs.map((doc) => doc.data()).toList();

          tasks.sort((a, b) {
            if (a.isFavorite && !a.isCompleted && (!b.isFavorite || b.isCompleted)) {
              return -1;
            }
            if (b.isFavorite && !b.isCompleted && (!a.isFavorite || a.isCompleted)) {
              return 1;
            }
            if (!a.isCompleted && b.isCompleted) {
              return -1;
            }
            if (a.isCompleted && !b.isCompleted) {
              return 1;
            }
            return b.createdAt.compareTo(a.createdAt);
          });

          return ListView.builder(
            itemCount: tasks.length,
            itemBuilder: (context, index) {
              final task = tasks[index];
              return Card(
                margin: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                elevation: 2,
                child: ListTile(
                  leading: IconButton(
                    icon: Icon(
                      task.isCompleted ? Icons.check_circle : Icons.circle_outlined,
                      color: task.isCompleted ? Colors.green : Colors.grey,
                    ),
                    onPressed: () => _toggleComplete(task),
                  ),
                  title: Text(
                    task.title,
                    style: TextStyle(
                      decoration: task.isCompleted ? TextDecoration.lineThrough : TextDecoration.none,
                      color: task.isCompleted ? Colors.grey : Colors.black,
                    ),
                  ),
                  subtitle: task.description.isNotEmpty
                      ? Text(
                          task.description,
                          style: TextStyle(
                            decoration: task.isCompleted ? TextDecoration.lineThrough : TextDecoration.none,
                            color: task.isCompleted ? Colors.grey[600] : Colors.grey[800],
                          ),
                          maxLines: 1,
                          overflow: TextOverflow.ellipsis,
                        )
                      : null,
                  trailing: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      IconButton(
                        icon: Icon(
                          task.isFavorite ? Icons.star : Icons.star_border,
                          color: task.isFavorite ? Colors.amber : Colors.grey,
                        ),
                        onPressed: () => _toggleFavorite(task),
                      ),
                      IconButton(
                        icon: const Icon(Icons.edit, color: Colors.blue),
                        onPressed: () => _editTask(task),
                      ),
                      IconButton(
                        icon: const Icon(Icons.delete, color: Colors.red),
                        onPressed: () => _deleteTask(task),
                      ),
                    ],
                  ),
                  onTap: () {
                    _editTask(task);
                  },
                ),
              );
            },
          );
        },
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: _addTask,
        child: const Icon(Icons.add),
      ),
    );
  }
}