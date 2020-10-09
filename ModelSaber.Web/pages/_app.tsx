import { AppProps, Container } from 'next/app'
import { AuthProvider } from '../components/store'

function App({ Component, pageProps }: AppProps) {
  return (
    <AuthProvider>
      <Component {...pageProps} />
    </AuthProvider>
  )
}

export default App