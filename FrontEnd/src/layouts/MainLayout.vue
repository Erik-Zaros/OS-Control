<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated class="bg-primary text-white">
      <q-toolbar>
        <q-btn flat dense round icon="menu" @click="toggleMini" />
        <q-toolbar-title>{{ pageTitle }}</q-toolbar-title>
        <q-space />
        <q-btn flat dense round :icon="$q.dark.isActive ? 'light_mode' : 'dark_mode'" @click="toggleDarkMode" />
        <q-btn flat dense round icon="account_circle" @click="loadUserData(); perfilDialog = true" />
      </q-toolbar>
    </q-header>

    <q-drawer
      v-model="leftDrawerOpen"
      show-if-above
      :mini="miniState && !isHovering"
      @mouseover="isHovering = true"
      @mouseout="isHovering = false"
      bordered
    >
      <div class="q-pa-md flex flex-center">
        <q-img
          :src="logoSidebar"
          spinner-color="primary"
          style="width: 100%; max-width: 120px"
        />
      </div>

<q-list padding class="drawer-list text-grey-8">
  <q-expansion-item icon="dashboard" label="Dashboard" dense default-opened>
    <q-item clickable to="/" exact>
      <q-item-section>Visão Geral</q-item-section>
    </q-item>
  </q-expansion-item>

  <q-expansion-item icon="group" label="Gestão de Usuários" dense>
    <q-item clickable to="/tecnicos">
      <q-item-section>Técnicos</q-item-section>
    </q-item>
  </q-expansion-item>

  <q-expansion-item icon="app_registration" label="Cadastro" dense>
    <q-item clickable to="/clientes">
      <q-item-section>Clientes</q-item-section>
    </q-item>
    <q-item clickable to="/produtos">
      <q-item-section>Produtos</q-item-section>
    </q-item>
  </q-expansion-item>

  <q-expansion-item icon="build" label="Ordens de Serviço" dense>
    <q-item clickable to="/cadastro-os">
      <q-item-section>Cadastrar OS</q-item-section>
    </q-item>
    <q-item clickable to="/consulta-os">
      <q-item-section>Consultar OS</q-item-section>
    </q-item>
  </q-expansion-item>

  <q-expansion-item icon="event" label="Agenda" dense>
    <q-item clickable to="/agenda">
      <q-item-section>Agenda dos Técnicos</q-item-section>
    </q-item>
  </q-expansion-item>

  <q-expansion-item icon="cloud_upload" label="Integrações" dense>
    <q-item clickable to="/integracoes/checkin">
      <q-item-section>Exportar para Checkin</q-item-section>
    </q-item>
  </q-expansion-item>
</q-list>

    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>

    <q-dialog v-model="perfilDialog">
      <q-card style="min-width: 350px">
        <q-card-section class="row items-center q-pb-none">
          <div class="text-h6">Perfil do Usuário</div>
          <q-space />
          <q-btn icon="close" flat round dense v-close-popup />
        </q-card-section>
        <q-card-section>
          <q-list bordered>
            <q-item>
              <q-item-section avatar>
                <q-icon name="person" />
              </q-item-section>
              <q-item-section>
                <q-item-label>{{ usuario.nome }}</q-item-label>
                <q-item-label caption>{{ usuario.email }}</q-item-label>
              </q-item-section>
            </q-item>
            <q-item>
              <q-item-section avatar>
                <q-icon name="work" />
              </q-item-section>
              <q-item-section>
                <q-item-label>Tipo: {{ usuario.tipoUsuario }}</q-item-label>
              </q-item-section>
            </q-item>
          </q-list>
        </q-card-section>
      </q-card>
    </q-dialog>
  </q-layout>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useQuasar } from 'quasar'
import { useRoute } from 'vue-router'

import logoLight from 'src/assets/serviceCore.png'
import logoDark from 'src/assets/serviceCoreDark.png'
import logoMiniLight from 'src/assets/serviceCoreMenor.png'
import logoMiniDark from 'src/assets/serviceCoreMenorDark.png'

const $q = useQuasar()
const route = useRoute()

const leftDrawerOpen = ref(true)
const miniState = ref(false)
const isHovering = ref(false)
const perfilDialog = ref(false)
const usuario = ref({
  nome: '',
  email: '',
  tipoUsuario: ''
})

const loadUserData = () => {
  const usuarioData = localStorage.getItem('usuario')
  if (usuarioData) {
    usuario.value = JSON.parse(usuarioData)
  }
}

watch(perfilDialog, (newValue) => {
  if (newValue) {
    loadUserData()
  }
})

onMounted(() => {
  loadUserData()
})

function toggleMini() {
  miniState.value = !miniState.value
}

function toggleDarkMode() {
  $q.dark.toggle()
}

const pageTitle = computed(() => {
  switch (route.path) {
    case '/': return 'Menu'
    case '/clientes': return 'Cadastro de Cliente'
    case '/produtos': return 'Cadastro de Produto'
    default: return 'Projeto OS'
  }
})

const logoSidebar = computed(() => {
  if ($q.dark.isActive) {
    return miniState.value && !isHovering.value ? logoMiniDark : logoDark
  } else {
    return miniState.value && !isHovering.value ? logoMiniLight : logoLight
  }
})
</script>

<style scoped>
.q-drawer {
  width: 180px !important;
}
.q-toolbar-title {
  font-weight: bold;
}
.drawer-text {
  font-size: 16px;
  font-weight: 500;
}
</style>