const currencyFormatter = new Intl.NumberFormat('nl-BE', {
  style: 'currency',
  currency: 'EUR',
})

export function formatCurrency(value: number): string {
  return currencyFormatter.format(value)
}

export function formatDate(value: string): string {
  const date = new Date(value)

  if (Number.isNaN(date.getTime())) {
    return value
  }

  return new Intl.DateTimeFormat('nl-BE').format(date)
}
