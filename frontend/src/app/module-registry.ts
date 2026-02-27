import type { RouteObject } from 'react-router-dom'
import { bookKeepingModule } from '@modules/book-keeping'

export type AppModule = {
  id: string
  title: string
  navItems: Array<{
    label: string
    to: string
  }>
  routes: RouteObject[]
}

export const modules: AppModule[] = [bookKeepingModule]

export const navigationItems = modules.flatMap((module) => module.navItems)
export const moduleRoutes = modules.flatMap((module) => module.routes)
