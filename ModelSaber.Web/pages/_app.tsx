import { AppProps } from 'next/app'
import { AuthProvider } from '../components/store'

import '../styles/light.scss'

function App({ Component, pageProps }: AppProps) {
    return (
        <AuthProvider>
            <Component {...pageProps} />
        </AuthProvider>
    )
}

export default App