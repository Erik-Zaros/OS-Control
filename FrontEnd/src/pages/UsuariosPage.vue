<template>
  <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
    <div class="row items-center q-col-gutter-md q-mb-md">
      <div class="col-12 col-md-6">
        <div class="text-h6">Cadastro de Usuários</div>
      </div>
    </div>

    <form-usuario @usuario-cadastrado="atualizarUsuarios" :usuarioParaEditar="usuarioEdicao" :loading="loading" />

    <q-separator class="q-my-md" />

    <tabela-usuarios :usuarios="usuarios" :loading="loading" @editar-usuario="editarUsuario" @toggle-ativo="toggleAtivo"
      @excluir-usuario="excluirUsuario" />

    <q-inner-loading :showing="loading">
      <q-spinner-gears size="50px" color="primary" />
    </q-inner-loading>
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useQuasar } from 'quasar'
import { api } from 'boot/axios'
import FormUsuario from 'src/components/usuarios/FormUsuario.vue'
import TabelaUsuarios from 'src/components/usuarios/TabelaUsuarios.vue'

const $q = useQuasar()
const usuarios = ref([])
const usuarioEdicao = ref(null)
const loading = ref(false)

async function carregarUsuarios() {
  loading.value = true
  try {
    const response = await api.get('/Usuario')
    usuarios.value = response.data || []
  } catch (error) {
    console.error('Erro ao carregar usuários:', error)
    $q.notify({
      type: 'negative',
      message: 'Erro ao carregar usuários'
    })
  } finally {
    loading.value = false
  }
}

async function atualizarUsuarios(usuario) {
  try {
    if (usuario && usuario.id) {
      const response = await api.put(`/Usuario/${usuario.id}`, usuario)
      const idx = usuarios.value.findIndex(u => u.id === usuario.id)
      if (idx !== -1) {
        usuarios.value[idx] = response.data
      }
      $q.notify({
        type: 'positive',
        message: 'Usuário atualizado com sucesso!'
      })
    } else {
      const response = await api.post('/Usuario', usuario)
      usuarios.value.push(response.data)
      $q.notify({
        type: 'positive',
        message: 'Usuário criado com sucesso!'
      })
    }
    usuarioEdicao.value = null
  } catch (error) {
    console.error('Erro ao salvar usuário:', error)
    $q.notify({
      type: 'negative',
      message: error.response?.data?.message || 'Erro ao salvar usuário'
    })
  }
}

function editarUsuario(usuario) {
  usuarioEdicao.value = { ...usuario }
}

async function toggleAtivo(usuario) {
  try {
    if (usuario.ativo) {
      await api.patch(`/Usuario/${usuario.id}/inativar`)
      $q.notify({
        type: 'positive',
        message: 'Usuário inativado com sucesso!'
      })
    } else {
      await api.patch(`/Usuario/${usuario.id}/ativar`)
      $q.notify({
        type: 'positive',
        message: 'Usuário ativado com sucesso!'
      })
    }

    const idx = usuarios.value.findIndex(u => u.id === usuario.id)
    if (idx !== -1) {
      usuarios.value[idx].ativo = !usuarios.value[idx].ativo
    }
  } catch (error) {
    console.error('Erro ao alterar status:', error)
    $q.notify({
      type: 'negative',
      message: 'Erro ao alterar status do usuário'
    })
  }
}

async function excluirUsuario(usuario) {
  $q.dialog({
    title: 'Confirmar exclusão',
    message: `Deseja realmente excluir o usuário ${usuario.nome}?`,
    cancel: true,
    persistent: true
  }).onOk(async () => {
    try {
      await api.delete(`/Usuario/${usuario.id}`)
      const idx = usuarios.value.findIndex(u => u.id === usuario.id)
      if (idx !== -1) {
        usuarios.value.splice(idx, 1)
      }
      $q.notify({
        type: 'positive',
        message: 'Usuário excluído com sucesso!'
      })
    } catch (error) {
      console.error('Erro ao excluir usuário:', error)
      $q.notify({
        type: 'negative',
        message: 'Erro ao excluir usuário'
      })
    }
  })
}

onMounted(() => {
  carregarUsuarios()
})
</script>