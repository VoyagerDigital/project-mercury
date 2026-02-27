import type { AppModule } from '@app/module-registry'
import { createElement } from 'react'
import { TransactionListPage } from '@modules/book-keeping/pages/transaction-list-page'
import { TransactionUpsertPage } from '@modules/book-keeping/pages/transaction-upsert-page'

export const bookKeepingModule: AppModule = {
  id: 'book-keeping',
  title: 'BookKeeping',
  navItems: [
    {
      label: 'Transactions',
      to: '/book-keeping/transactions',
    },
  ],
  routes: [
    {
      path: '/book-keeping/transactions',
      element: createElement(TransactionListPage),
    },
    {
      path: '/book-keeping/transactions/new',
      element: createElement(TransactionUpsertPage),
    },
    {
      path: '/book-keeping/transactions/:id',
      element: createElement(TransactionUpsertPage),
    },
  ],
}
