<template>
  <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
    <div class="row items-center q-col-gutter-md q-mb-md">
      <div class="col-12 col-md-6">
        <div class="text-h6">Cadastro de Usuários</div>
      </div>
    </div>

    <form-usuario @usuario-cadastrado="atualizarUsuarios" :usuarioParaEditar="usuarioEdicao" />

    <q-separator class="q-my-md" />

    <tabela-usuarios :usuarios="usuarios" @editar-usuario="editarUsuario" @toggle-ativo="toggleAtivo" />
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import FormUsuario from 'src/components/usuarios/FormUsuario.vue'
import TabelaUsuarios from 'src/components/usuarios/TabelaUsuarios.vue'

// Dados fictícios
const usuarios = ref([
  {
    id: 1,
    nome: 'Marcelo Souza',
    funcao: 'Técnico',
    login: 'marcelo',
    senha: '',
    senha2: '',
    email: 'marcelo@empresa.com',
    telefone: '(14) 98765-1234',
    tipousuario: 'Técnico',
    ultimoacesso: '2024-11-11 12:15',
    especialidade: 'Elétrica',
    ativo: true
  },
  {
    id: 2,
    nome: 'Patrícia Lima',
    funcao: 'Gestora',
    login: 'patricia',
    senha: '',
    senha2: '',
    email: 'patricia@empresa.com',
    telefone: '(14) 93456-7890',
    tipousuario: 'Administrador',
    ultimoacesso: '2024-12-05 08:22',
    especialidade: 'Gestão',
    ativo: true
  },
  {
    id: 3,
    nome: 'Rafael Santos',
    funcao: 'TI',
    login: 'rafael',
    senha: '',
    senha2: '',
    email: 'rafael@empresa.com',
    telefone: '(14) 90012-4567',
    tipousuario: 'Operador',
    ultimoacesso: '2024-11-20 15:40',
    especialidade: 'TI',
    ativo: false
  }
])

const usuarioEdicao = ref(null)

function atualizarUsuarios(usuario) {

  if (usuario && usuario.id) {
    const idx = usuarios.value.findIndex(u => u.id === usuario.id)
    if (idx !== -1) {
      usuarios.value[idx] = { ...usuario }
    } else {
      usuarios.value.push({ ...usuario })
    }
  }
  usuarioEdicao.value = null
}

function editarUsuario(usuario) {
  usuarioEdicao.value = { ...usuario }
}

function toggleAtivo(usuario) {
  // Alterar o status de ativo no array (dado fictício)
  const idx = usuarios.value.findIndex(u => u.id === usuario.id)
  if (idx !== -1) {
    usuarios.value[idx].ativo = !usuarios.value[idx].ativo
  }
}

onMounted(() => {
  // Carrega api
})
</script>
