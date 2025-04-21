<template>
  <div class="perfil-usuario">
    <div class="q-pa-md">
      <div class="row items-center q-mb-md">
        <q-avatar size="50px" color="primary" text-color="white">
          {{ iniciais }}
        </q-avatar>
        <div class="q-ml-md">
          <div class="text-h6">{{ usuario.nome }}</div>
          <div class="text-subtitle2">{{ usuario.email }}</div>
        </div>
      </div>

      <q-list>
        <q-item>
          <q-item-section avatar>
            <q-icon name="person" />
          </q-item-section>
          <q-item-section>
            <q-item-label>Login</q-item-label>
            <q-item-label caption>{{ usuario.login }}</q-item-label>
          </q-item-section>
        </q-item>

        <q-item>
          <q-item-section avatar>
            <q-icon name="badge" />
          </q-item-section>
          <q-item-section>
            <q-item-label>Tipo</q-item-label>
            <q-item-label caption>{{ usuario.tipoUsuario }}</q-item-label>
          </q-item-section>
        </q-item>
      </q-list>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'

const usuario = ref({
  nome: '',
  email: '',
  login: '',
  tipoUsuario: ''
})

const iniciais = computed(() => {
  if (!usuario.value.nome) return ''
  return usuario.value.nome
    .split(' ')
    .map(n => n[0])
    .join('')
    .toUpperCase()
    .substring(0, 2)
})

onMounted(() => {
  const usuarioData = localStorage.getItem('usuario')
  if (usuarioData) {
    usuario.value = JSON.parse(usuarioData)
  }
})
</script>

<style scoped>
.perfil-usuario {
  min-width: 300px;
}
</style> 