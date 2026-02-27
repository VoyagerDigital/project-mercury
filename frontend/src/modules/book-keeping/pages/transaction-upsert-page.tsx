import { Link, useNavigate, useParams } from 'react-router-dom'
import { Card, CardContent, CardHeader, CardTitle } from '@components/ui/card'
import { TransactionForm } from '@modules/book-keeping/components/transaction-form'
import {
  useCreateTransaction,
  useTransaction,
  useUpdateTransaction,
} from '@modules/book-keeping/hooks/use-transactions'

export function TransactionUpsertPage() {
  const navigate = useNavigate()
  const { id } = useParams()

  const isCreateMode = id === 'new'
  const parsedId = Number(id)

  const transactionQuery = useTransaction(parsedId, !isCreateMode && Number.isInteger(parsedId))
  const createMutation = useCreateTransaction()
  const updateMutation = useUpdateTransaction(parsedId)

  if (!isCreateMode && !Number.isInteger(parsedId)) {
    return <p className="text-sm text-destructive">Invalid transaction id.</p>
  }

  return (
    <div className="grid gap-6">
      <div className="flex items-center justify-between gap-4">
        <div>
          <h2 className="text-2xl font-bold">{isCreateMode ? 'Create transaction' : 'Edit transaction'}</h2>
          <p className="text-sm text-muted-foreground">BookKeeping module</p>
        </div>

        <Link
          to="/book-keeping/transactions"
          className="inline-flex h-10 items-center justify-center rounded-md bg-secondary px-4 py-2 text-sm font-medium text-secondary-foreground transition-colors hover:opacity-90"
        >
          Back
        </Link>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>{isCreateMode ? 'New Transaction' : `Transaction #${parsedId}`}</CardTitle>
        </CardHeader>
        <CardContent>
          {transactionQuery.isLoading ? (
            <p className="text-sm text-muted-foreground">Loading transaction...</p>
          ) : (
            <TransactionForm
              initialValue={transactionQuery.data?.transaction}
              isSubmitting={createMutation.isPending || updateMutation.isPending}
              onSubmit={async (value) => {
                if (isCreateMode) {
                  await createMutation.mutateAsync(value)
                } else {
                  await updateMutation.mutateAsync(value)
                }

                await navigate('/book-keeping/transactions')
              }}
            />
          )}
        </CardContent>
      </Card>
    </div>
  )
}
