const routes = [
  {
    path: '/',
    redirect: '/login'
  },
  {
    path: '/dashboard',
    component: () => import('layouts/MainLayout.vue'),
    meta: { requiresAuth: true },
    children: [
      { path: '', component: () => import('pages/IndexPage.vue') },
      { path: 'clientes', component: () => import('pages/ClientesPage.vue') },
      { path: 'produtos', component: () => import('pages/ProdutosPage.vue') },
      { path: 'cadastro-ticket', component: () => import('src/pages/CadastroTicketPage.vue') },
      { path: 'consulta-ticket', component: () => import('src/pages/ConsultaTicket.vue') },
      { path: 'ticket/:numero', component: () => import('src/pages/VisualizarTicketPage.vue') },
      { path: 'agenda', component: () => import('pages/AgendaPage.vue') },
      { path: 'usuarios', component: () => import('pages/UsuariosPage.vue') },
      { path: 'integracoes/checkin', component: () => import('pages/CheckinPage.vue') }
    ]
  },
  {
    path: '/login',
    component: () => import('layouts/LoginLayout.vue'),
    children: [
      { path: '', component: () => import('pages/LoginPage.vue') }
    ]
  },
  {
    path: '/cadastro-usuario',
    component: () => import('layouts/LoginLayout.vue'),
    children: [
      { path: '', component: () => import('pages/CadastroUsuarioPage.vue') }
    ]
  },  
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue')
  }
]

export default routes
