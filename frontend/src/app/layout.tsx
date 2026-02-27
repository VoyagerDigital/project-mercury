import { NavLink, Outlet } from 'react-router-dom'
import { navigationItems } from '@app/module-registry'
import { ThemeToggle } from '@components/theme-toggle'
import { cn } from '@lib/utils'

export function RootLayout() {
  return (
    <div className="grid min-h-screen grid-cols-1 md:grid-cols-[240px_1fr]">
      <aside className="border-r border-border bg-card p-4">
        <h1 className="mb-4 text-xl font-bold">Mercury</h1>

        <nav className="flex flex-col gap-2">
          {navigationItems.map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              className={({ isActive }) =>
                cn(
                  'rounded-md px-3 py-2 text-sm font-medium transition-colors hover:bg-secondary',
                  isActive ? 'bg-secondary text-secondary-foreground' : 'text-muted-foreground',
                )
              }
            >
              {item.label}
            </NavLink>
          ))}
        </nav>
      </aside>

      <main className="flex flex-col">
        <header className="flex h-16 items-center justify-end border-b border-border px-6">
          <ThemeToggle />
        </header>

        <section className="flex-1 p-6">
          <Outlet />
        </section>
      </main>
    </div>
  )
}
