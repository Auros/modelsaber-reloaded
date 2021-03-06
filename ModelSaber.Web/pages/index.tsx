import Head from 'next/head'
import Navbar from '../components/navbar'
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
            <Navbar />
            <div className="container">
                <div className="box">
                    <p>Token: {auth.token}</p>
                    <div className="buttons">
                        <button className="button is-primary" onClick={handleLogin} disabled={auth.authenticated}>Log In</button>
                        <button className="button is-info" onClick={handleLogout} disabled={!auth.authenticated}>Log Out</button>
                    </div>
                </div>
            </div>
        </>
    )
}

export default Index