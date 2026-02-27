import { Moon, Sun, MonitorCog } from 'lucide-react'
import { useTheme } from '@components/theme-provider'

const themes = [
  { value: 'light', label: 'Light', icon: Sun },
  { value: 'dark', label: 'Dark', icon: Moon },
  { value: 'system', label: 'System', icon: MonitorCog },
] as const

export function ThemeToggle() {
  const { theme, setTheme } = useTheme()

  return (
    <div className="inline-flex items-center gap-1 rounded-md border border-border p-1">
      {themes.map((themeOption) => {
        const Icon = themeOption.icon

        return (
          <button
            key={themeOption.value}
            type="button"
            className={
              theme === themeOption.value
                ? 'inline-flex items-center gap-1 rounded px-2 py-1 text-xs font-medium bg-secondary text-secondary-foreground'
                : 'inline-flex items-center gap-1 rounded px-2 py-1 text-xs text-muted-foreground hover:text-foreground'
            }
            onClick={() => setTheme(themeOption.value)}
          >
            <Icon className="size-3.5" />
            {themeOption.label}
          </button>
        )
      })}
    </div>
  )
}
