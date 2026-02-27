import { createBrowserRouter, Navigate } from 'react-router-dom'
import { RootLayout } from '@app/layout'
import { moduleRoutes } from '@app/module-registry'

export const router = createBrowserRouter([
  {
    path: '/',
    element: <RootLayout />,
    children: [
      {
        index: true,
        element: <Navigate to="/book-keeping/transactions" replace />,
      },
      ...moduleRoutes,
    ],
  },
])
