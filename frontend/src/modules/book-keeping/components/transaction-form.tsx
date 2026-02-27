import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import { z } from 'zod'
import { Button } from '@components/ui/button'
import { Input } from '@components/ui/input'
import type { CreateTransactionRequest } from '@modules/book-keeping/api/contracts'
import type { Transaction } from '@modules/book-keeping/types/transaction'
import { TransactionType } from '@modules/book-keeping/types/transaction'

const transactionSchema = z.object({
  description: z.string().min(1, 'Description is required').max(200, 'Description can be max 200 characters'),
  amount: z
    .string()
    .min(1, 'Amount is required')
    .refine((value) => Number(value) > 0, 'Amount must be greater than 0'),
  date: z.string().min(1, 'Date is required'),
  type: z.enum([String(TransactionType.Income), String(TransactionType.Expense)]),
})

type TransactionFormValues = z.infer<typeof transactionSchema>

type TransactionFormProps = {
  initialValue?: Transaction
  isSubmitting: boolean
  onSubmit: (value: CreateTransactionRequest) => Promise<void>
}

export function TransactionForm({ initialValue, isSubmitting, onSubmit }: TransactionFormProps) {
  const form = useForm<TransactionFormValues>({
    resolver: zodResolver(transactionSchema),
    defaultValues: {
      description: initialValue?.description ?? '',
      amount: String(initialValue?.amount ?? ''),
      date: initialValue?.date ?? new Date().toISOString().slice(0, 10),
      type: String(initialValue?.transactionType ?? TransactionType.Expense),
    },
  })

  return (
    <form
      className="grid gap-4"
      onSubmit={form.handleSubmit(async (value) => {
        await onSubmit({
          description: value.description,
          amount: Number(value.amount),
          date: value.date,
          type: Number(value.type) as CreateTransactionRequest['type'],
        })
      })}
    >
      <div className="grid gap-2">
        <label htmlFor="description" className="text-sm font-medium">
          Description
        </label>
        <Input id="description" {...form.register('description')} />
        {form.formState.errors.description ? (
          <p className="text-xs text-destructive">{form.formState.errors.description.message}</p>
        ) : null}
      </div>

      <div className="grid gap-2 md:grid-cols-2">
        <div className="grid gap-2">
          <label htmlFor="amount" className="text-sm font-medium">
            Amount
          </label>
          <Input id="amount" type="number" step="0.01" {...form.register('amount')} />
          {form.formState.errors.amount ? (
            <p className="text-xs text-destructive">{form.formState.errors.amount.message}</p>
          ) : null}
        </div>

        <div className="grid gap-2">
          <label htmlFor="date" className="text-sm font-medium">
            Date
          </label>
          <Input id="date" type="date" {...form.register('date')} />
          {form.formState.errors.date ? <p className="text-xs text-destructive">{form.formState.errors.date.message}</p> : null}
        </div>
      </div>

      <div className="grid gap-2">
        <label htmlFor="type" className="text-sm font-medium">
          Type
        </label>
        <select
          id="type"
          className="h-10 rounded-md border border-input bg-background px-3 text-sm"
          {...form.register('type')}
        >
          <option value={String(TransactionType.Expense)}>Expense</option>
          <option value={String(TransactionType.Income)}>Income</option>
        </select>
      </div>

      <Button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Saving...' : 'Save transaction'}
      </Button>
    </form>
  )
}
