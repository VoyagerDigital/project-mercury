import { Input } from '@components/ui/input'

type TransactionFiltersProps = {
  searchterm: string
  pageSize: number
  onSearchtermChange: (value: string) => void
  onPageSizeChange: (value: number) => void
}

export function TransactionFilters({
  searchterm,
  pageSize,
  onSearchtermChange,
  onPageSizeChange,
}: TransactionFiltersProps) {
  return (
    <div className="grid gap-3 md:grid-cols-[1fr_140px]">
      <Input
        placeholder="Search description..."
        value={searchterm}
        onChange={(event) => onSearchtermChange(event.target.value)}
      />

      <select
        value={pageSize}
        onChange={(event) => onPageSizeChange(Number(event.target.value))}
        className="h-10 rounded-md border border-input bg-background px-3 text-sm"
      >
        {[10, 20, 50].map((value) => (
          <option key={value} value={value}>
            {value} / page
          </option>
        ))}
      </select>
    </div>
  )
}
