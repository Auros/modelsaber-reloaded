import { useReducer, useContext, createContext, ReducerAction } from 'react'
import { AuthState, defaultAuth } from '../types/auth'
const AuthStateContext = createContext(null)
const AuthDispatchContext = createContext(null)

const reducer = (_state: any, action: { type: string; payload: string }) => {
    switch (action.type) {
        case 'LOGGED_IN':
            const auth: AuthState = { token: action.payload, authenticated: true }
            return auth
        case 'LOG_OUT':
            return defaultAuth
        default:
            throw new Error(`Unknown Action: ${action.type}`)
    }
}

export const AuthProvider = ({ children }) => {
    const [state, dispatch] = useReducer(reducer, defaultAuth)
    return (
        <AuthDispatchContext.Provider value={dispatch}>
            <AuthStateContext.Provider value={state}>
                {children}
            </AuthStateContext.Provider>
        </AuthDispatchContext.Provider>
    )
}

export const useAuth = () => useContext(AuthStateContext)
export const useDispatchAuth = () => useContext(AuthDispatchContext)