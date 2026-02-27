import { useMemo, useState } from 'react'
import { Link } from 'react-router-dom'
import { Button } from '@components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@components/ui/card'
import { TransactionFilters } from '@modules/book-keeping/components/transaction-filters'
import { TransactionTable } from '@modules/book-keeping/components/transaction-table'
import { TransactionType } from '@modules/book-keeping/types/transaction'
import { useDeleteTransaction, useTransactions } from '@modules/book-keeping/hooks/use-transactions'
import { formatCurrency, formatDate } from '@shared/utils/format'

export function TransactionListPage() {
  const [searchterm, setSearchterm] = useState('')
  const [page, setPage] = useState(1)
  const [pageSize, setPageSize] = useState(10)

  const request = useMemo(
    () => ({
      searchterm,
      page,
      pageSize,
    }),
    [searchterm, page, pageSize],
  )

  const transactionsQuery = useTransactions(request)
  const deleteTransactionMutation = useDeleteTransaction()
  const transactions = transactionsQuery.data?.transactions ?? []

  const totalCount = transactionsQuery.data?.totalCount ?? 0
  const totalPages = Math.max(1, Math.ceil(totalCount / pageSize))

  const dashboardMetrics = useMemo(() => {
    const income = transactions
      .filter((transaction) => transaction.transactionType === TransactionType.Income)
      .reduce((sum, transaction) => sum + transaction.amount, 0)

    const expenses = transactions
      .filter((transaction) => transaction.transactionType === TransactionType.Expense)
      .reduce((sum, transaction) => sum + transaction.amount, 0)

    const netCashFlow = income - expenses
    const savingsRate = income > 0 ? (netCashFlow / income) * 100 : 0

    const largestExpense = transactions
      .filter((transaction) => transaction.transactionType === TransactionType.Expense)
      .reduce((largest, current) => (current.amount > largest.amount ? current : largest), {
        id: -1,
        description: 'No expenses yet',
        amount: 0,
        date: '',
        transactionType: TransactionType.Expense,
      })

    const monthlyTotals = transactions.reduce<
      Record<string, { label: string; income: number; expenses: number; net: number }>
    >((accumulator, transaction) => {
      const date = new Date(transaction.date)

      if (Number.isNaN(date.getTime())) {
        return accumulator
      }

      const monthKey = `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}`
      const monthLabel = date.toLocaleDateString('nl-BE', { month: 'short', year: 'numeric' })

      if (!accumulator[monthKey]) {
        accumulator[monthKey] = {
          label: monthLabel,
          income: 0,
          expenses: 0,
          net: 0,
        }
      }

      if (transaction.transactionType === TransactionType.Income) {
        accumulator[monthKey].income += transaction.amount
        accumulator[monthKey].net += transaction.amount
      } else {
        accumulator[monthKey].expenses += transaction.amount
        accumulator[monthKey].net -= transaction.amount
      }

      return accumulator
    }, {})

    const monthlySeries = Object.entries(monthlyTotals)
      .sort(([left], [right]) => left.localeCompare(right))
      .map(([, value]) => value)

    let runningBalance = 0
    const balanceTrend = monthlySeries.map((entry) => {
      runningBalance += entry.net
      return {
        label: entry.label,
        value: runningBalance,
      }
    })

    const maxMonthlyValue = Math.max(1, ...monthlySeries.map((entry) => Math.max(entry.income, entry.expenses)))

    const chartWidth = 360
    const chartHeight = 180
    const chartPadding = 16
    const xSpan = chartWidth - chartPadding * 2
    const ySpan = chartHeight - chartPadding * 2

    const minTrendValue = Math.min(0, ...balanceTrend.map((entry) => entry.value))
    const maxTrendValue = Math.max(0, ...balanceTrend.map((entry) => entry.value))
    const trendValueSpan = Math.max(1, maxTrendValue - minTrendValue)

    const balanceTrendPoints = balanceTrend
      .map((entry, index) => {
        const x =
          balanceTrend.length <= 1
            ? chartWidth / 2
            : chartPadding + (index / (balanceTrend.length - 1)) * xSpan
        const y = chartPadding + ((maxTrendValue - entry.value) / trendValueSpan) * ySpan

        return `${x},${y}`
      })
      .join(' ')

    const totalMovement = income + expenses
    const incomeShare = totalMovement > 0 ? (income / totalMovement) * 100 : 0

    const recentActivity = [...transactions]
      .sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime())
      .slice(0, 5)

    return {
      income,
      expenses,
      netCashFlow,
      savingsRate,
      largestExpense,
      monthlySeries,
      maxMonthlyValue,
      balanceTrend,
      balanceTrendPoints,
      incomeShare,
      recentActivity,
    }
  }, [transactions])

  return (
    <div className="grid gap-6">
      <div className="flex items-center justify-between gap-4">
        <div>
          <h2 className="text-2xl font-bold">Transactions</h2>
          <p className="text-sm text-muted-foreground">Financial dashboard for BookKeeping transactions.</p>
        </div>

        <Link
          to="/book-keeping/transactions/new"
          className="inline-flex h-10 items-center justify-center rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground transition-colors hover:opacity-90"
        >
          New transaction
        </Link>
      </div>

      <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
        <Card>
          <CardHeader>
            <CardTitle className="text-sm font-medium text-muted-foreground">Income</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-2xl font-semibold">{formatCurrency(dashboardMetrics.income)}</p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="text-sm font-medium text-muted-foreground">Expenses</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-2xl font-semibold">{formatCurrency(dashboardMetrics.expenses)}</p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="text-sm font-medium text-muted-foreground">Net cash flow</CardTitle>
          </CardHeader>
          <CardContent>
            <p
              className={`text-2xl font-semibold ${dashboardMetrics.netCashFlow >= 0 ? 'text-green-600' : 'text-red-600'}`}
            >
              {formatCurrency(dashboardMetrics.netCashFlow)}
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="text-sm font-medium text-muted-foreground">Savings rate</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-2xl font-semibold">{dashboardMetrics.savingsRate.toFixed(1)}%</p>
          </CardContent>
        </Card>
      </div>

      <div className="grid gap-4 lg:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>Monthly cash flow</CardTitle>
          </CardHeader>
          <CardContent>
            {dashboardMetrics.monthlySeries.length === 0 ? (
              <p className="text-sm text-muted-foreground">Not enough data to render chart.</p>
            ) : (
              <div className="grid gap-3">
                {dashboardMetrics.monthlySeries.map((entry) => {
                  const incomeWidth = (entry.income / dashboardMetrics.maxMonthlyValue) * 100
                  const expenseWidth = (entry.expenses / dashboardMetrics.maxMonthlyValue) * 100

                  return (
                    <div key={entry.label} className="grid gap-1.5">
                      <div className="flex items-center justify-between text-xs text-muted-foreground">
                        <span>{entry.label}</span>
                        <span>{formatCurrency(entry.net)}</span>
                      </div>
                      <div className="space-y-1">
                        <div className="h-2 rounded bg-secondary/70">
                          <div className="h-2 rounded bg-green-500" style={{ width: `${incomeWidth}%` }} />
                        </div>
                        <div className="h-2 rounded bg-secondary/70">
                          <div className="h-2 rounded bg-red-500" style={{ width: `${expenseWidth}%` }} />
                        </div>
                      </div>
                    </div>
                  )
                })}
                <div className="flex items-center gap-4 pt-1 text-xs text-muted-foreground">
                  <div className="flex items-center gap-1">
                    <span className="inline-block h-2 w-2 rounded-full bg-green-500" /> Income
                  </div>
                  <div className="flex items-center gap-1">
                    <span className="inline-block h-2 w-2 rounded-full bg-red-500" /> Expense
                  </div>
                </div>
              </div>
            )}
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Balance trend</CardTitle>
          </CardHeader>
          <CardContent>
            {dashboardMetrics.balanceTrend.length < 2 ? (
              <p className="text-sm text-muted-foreground">Add transactions across multiple months to show a trend.</p>
            ) : (
              <div className="grid gap-2">
                <svg viewBox="0 0 360 180" className="h-44 w-full">
                  <line x1="16" y1="90" x2="344" y2="90" className="stroke-border" strokeWidth="1" />
                  <polyline
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="3"
                    className="text-primary"
                    points={dashboardMetrics.balanceTrendPoints}
                  />
                </svg>
                <div className="flex items-center justify-between text-xs text-muted-foreground">
                  <span>{dashboardMetrics.balanceTrend[0]?.label}</span>
                  <span>{dashboardMetrics.balanceTrend[dashboardMetrics.balanceTrend.length - 1]?.label}</span>
                </div>
              </div>
            )}
          </CardContent>
        </Card>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Insights</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid gap-6 lg:grid-cols-[220px_1fr]">
            <div className="flex flex-col items-center justify-center gap-3">
              <div
                className="h-36 w-36 rounded-full"
                style={{
                  background: `conic-gradient(rgb(34 197 94) 0% ${dashboardMetrics.incomeShare}%, rgb(239 68 68) ${dashboardMetrics.incomeShare}% 100%)`,
                }}
              />
              <div className="text-center text-xs text-muted-foreground">
                <p>Income vs expense split</p>
                <p>{dashboardMetrics.incomeShare.toFixed(1)}% income</p>
              </div>
            </div>

            <div className="grid gap-4">
              <div>
                <p className="text-sm text-muted-foreground">Largest expense</p>
                <p className="text-base font-medium">{dashboardMetrics.largestExpense.description}</p>
                <p className="text-sm text-muted-foreground">
                  {formatCurrency(dashboardMetrics.largestExpense.amount)}
                  {dashboardMetrics.largestExpense.date
                    ? ` • ${formatDate(dashboardMetrics.largestExpense.date)}`
                    : ''}
                </p>
              </div>

              <div>
                <p className="text-sm text-muted-foreground">Recent activity</p>
                {dashboardMetrics.recentActivity.length === 0 ? (
                  <p className="text-sm text-muted-foreground">No transactions yet.</p>
                ) : (
                  <div className="mt-2 grid gap-2">
                    {dashboardMetrics.recentActivity.map((transaction) => (
                      <div key={transaction.id} className="flex items-center justify-between text-sm">
                        <div>
                          <p className="font-medium">{transaction.description}</p>
                          <p className="text-xs text-muted-foreground">{formatDate(transaction.date)}</p>
                        </div>
                        <p
                          className={
                            transaction.transactionType === TransactionType.Income
                              ? 'font-medium text-green-600'
                              : 'font-medium text-red-600'
                          }
                        >
                          {transaction.transactionType === TransactionType.Income ? '+' : '-'}
                          {formatCurrency(transaction.amount)}
                        </p>
                      </div>
                    ))}
                  </div>
                )}
              </div>
            </div>
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Filters</CardTitle>
        </CardHeader>
        <CardContent>
          <TransactionFilters
            searchterm={searchterm}
            pageSize={pageSize}
            onSearchtermChange={(value) => {
              setSearchterm(value)
              setPage(1)
            }}
            onPageSizeChange={(value) => {
              setPageSize(value)
              setPage(1)
            }}
          />
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Transactions</CardTitle>
        </CardHeader>
        <CardContent>
          {transactionsQuery.isLoading ? (
            <p className="text-sm text-muted-foreground">Loading transactions...</p>
          ) : (
            <div className="grid gap-4">
              <TransactionTable
                transactions={transactions}
                onDelete={(id) => {
                  void deleteTransactionMutation.mutateAsync(id)
                }}
              />

              <div className="flex items-center justify-between text-sm">
                <p className="text-muted-foreground">
                  Page {page} of {totalPages}
                </p>

                <div className="flex gap-2">
                  <Button
                    variant="secondary"
                    size="sm"
                    disabled={page <= 1}
                    onClick={() => setPage((currentPage) => Math.max(1, currentPage - 1))}
                  >
                    Previous
                  </Button>
                  <Button
                    variant="secondary"
                    size="sm"
                    disabled={page >= totalPages}
                    onClick={() => setPage((currentPage) => Math.min(totalPages, currentPage + 1))}
                  >
                    Next
                  </Button>
                </div>
              </div>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  )
}
