import { httpClient } from '@shared/api/http-client'
import type {
  CreateTransactionRequest,
  ReadTransactionByIdResponse,
  ReadTransactionsRequest,
  ReadTransactionsResponse,
  UpdateTransactionRequest,
} from './contracts'

const resourcePath = '/transaction'

export async function readTransactions(request: ReadTransactionsRequest): Promise<ReadTransactionsResponse> {
  const { data } = await httpClient.get<ReadTransactionsResponse>(resourcePath, {
    params: {
      Searchterm: request.searchterm,
      Page: request.page,
      PageSize: request.pageSize,
    },
  })

  return data
}

export async function readTransactionById(id: number): Promise<ReadTransactionByIdResponse> {
  const { data } = await httpClient.get<ReadTransactionByIdResponse>(`${resourcePath}/${id}`)
  return data
}

export async function createTransaction(request: CreateTransactionRequest): Promise<number> {
  const { data } = await httpClient.post<number>(resourcePath, request)
  return data
}

export async function updateTransactionById(id: number, request: UpdateTransactionRequest): Promise<void> {
  await httpClient.put<void>(`${resourcePath}/${id}`, request)
}

export async function deleteTransactionById(id: number): Promise<void> {
  await httpClient.delete<void>(`${resourcePath}/${id}`)
}
