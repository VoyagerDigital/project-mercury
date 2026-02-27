import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import {
  createTransaction,
  deleteTransactionById,
  readTransactionById,
  readTransactions,
  updateTransactionById,
} from '@modules/book-keeping/api/transaction-api'
import type { CreateTransactionRequest, ReadTransactionsRequest, UpdateTransactionRequest } from '../api/contracts'

const queryKeys = {
  all: ['book-keeping', 'transactions'] as const,
  list: (request: ReadTransactionsRequest) => [...queryKeys.all, request] as const,
  byId: (id: number) => [...queryKeys.all, id] as const,
}

export function useTransactions(request: ReadTransactionsRequest) {
  return useQuery({
    queryKey: queryKeys.list(request),
    queryFn: () => readTransactions(request),
  })
}

export function useTransaction(id: number, enabled = true) {
  return useQuery({
    queryKey: queryKeys.byId(id),
    queryFn: () => readTransactionById(id),
    enabled,
  })
}

export function useCreateTransaction() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (request: CreateTransactionRequest) => createTransaction(request),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: queryKeys.all })
    },
  })
}

export function useUpdateTransaction(id: number) {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (request: UpdateTransactionRequest) => updateTransactionById(id, request),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: queryKeys.byId(id) })
      await queryClient.invalidateQueries({ queryKey: queryKeys.all })
    },
  })
}

export function useDeleteTransaction() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (id: number) => deleteTransactionById(id),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: queryKeys.all })
    },
  })
}
