export interface AuthState {
    token?: string
    authenticated: boolean
}

export const defaultAuth: AuthState = {
    token: null,
    authenticated: false
}