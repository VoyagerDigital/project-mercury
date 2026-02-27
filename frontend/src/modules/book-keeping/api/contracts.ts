import type { FilterableRequest } from '@shared/types/filterable-request'
import type { Transaction, TransactionType } from '@modules/book-keeping/types/transaction'

export type ReadTransactionsRequest = FilterableRequest

export type CreateTransactionRequest = {
  description: string
  amount: number
  date: string
  type: TransactionType
}

export type UpdateTransactionRequest = CreateTransactionRequest

export type ReadTransactionsResponse = {
  transactions: Transaction[]
  totalCount: number
}

export type ReadTransactionByIdResponse = {
  transaction: Transaction
}
