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
      { path: 'cadastro-os', component: () => import('pages/CadastroOrdemServicoPage.vue') },
      { path: 'consulta-os', component: () => import('pages/ConsultaOrdemServicoPage.vue') },
      { path: 'os/:numero', component: () => import('pages/VisualizarOrdemServicoPage.vue') }
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
