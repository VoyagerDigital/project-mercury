export const TransactionType = {
  Income: 0,
  Expense: 1,
} as const

export type TransactionType = (typeof TransactionType)[keyof typeof TransactionType]

export type Transaction = {
  id: number
  description: string
  amount: number
  date: string
  transactionType: TransactionType
}
