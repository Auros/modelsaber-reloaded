import Head from 'next/head'
import { useAuth, useDispatchAuth } from '../components/store'

const Index = () => {
  const auth = useAuth()
  const dispatch = useDispatchAuth()

  const handleLogin = (_event: any) => dispatch({ type: 'LOGGED_IN', payload: 'etf.my_token.xyz' })
  const handleLogout = (_event: any) => dispatch({ type: 'LOG_OUT' })

  return (
    <>
      <Head>
        <title>ModelSaber</title>
      </Head>
      <p>Token: {auth.token}</p>
      <button onClick={handleLogin}>Log In</button>
      <button onClick={handleLogout}>Log Out</button>
    </>
  )
}

export default Index