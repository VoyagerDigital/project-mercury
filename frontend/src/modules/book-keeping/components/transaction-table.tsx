import {
  createColumnHelper,
  flexRender,
  getCoreRowModel,
  useReactTable,
} from '@tanstack/react-table'
import { Link } from 'react-router-dom'
import { Button } from '@components/ui/button'
import type { Transaction } from '@modules/book-keeping/types/transaction'
import { formatCurrency, formatDate } from '@shared/utils/format'

const columnHelper = createColumnHelper<Transaction>()

type TransactionTableProps = {
  transactions: Transaction[]
  onDelete: (id: number) => void
}

export function TransactionTable({ transactions, onDelete }: TransactionTableProps) {
  const columns = [
    columnHelper.accessor('description', {
      header: 'Description',
      cell: (info) => info.getValue(),
    }),
    columnHelper.accessor('amount', {
      header: 'Amount',
      cell: (info) => formatCurrency(info.getValue()),
    }),
    columnHelper.accessor('date', {
      header: 'Date',
      cell: (info) => formatDate(info.getValue()),
    }),
    columnHelper.accessor('transactionType', {
      header: 'Type',
      cell: (info) => (info.getValue() === 0 ? 'Income' : 'Expense'),
    }),
    columnHelper.display({
      id: 'actions',
      header: '',
      cell: (info) => (
        <div className="flex justify-end gap-2">
          <Link
            to={`/book-keeping/transactions/${info.row.original.id}`}
            className="inline-flex h-9 items-center justify-center rounded-md bg-transparent px-3 text-sm font-medium hover:bg-secondary"
          >
            Edit
          </Link>
          <Button variant="destructive" size="sm" onClick={() => onDelete(info.row.original.id)}>
            Delete
          </Button>
        </div>
      ),
    }),
  ]

  const table = useReactTable({
    data: transactions,
    columns,
    getCoreRowModel: getCoreRowModel(),
  })

  return (
    <div className="overflow-hidden rounded-md border">
      <table className="w-full text-sm">
        <thead className="bg-muted">
          {table.getHeaderGroups().map((headerGroup) => (
            <tr key={headerGroup.id}>
              {headerGroup.headers.map((header) => (
                <th key={header.id} className="px-4 py-3 text-left font-medium">
                  {header.isPlaceholder ? null : flexRender(header.column.columnDef.header, header.getContext())}
                </th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody>
          {table.getRowModel().rows.length === 0 ? (
            <tr>
              <td colSpan={5} className="px-4 py-8 text-center text-muted-foreground">
                No transactions found
              </td>
            </tr>
          ) : (
            table.getRowModel().rows.map((row) => (
              <tr key={row.id} className="border-t">
                {row.getVisibleCells().map((cell) => (
                  <td key={cell.id} className="px-4 py-3 align-middle">
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </td>
                ))}
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  )
}
