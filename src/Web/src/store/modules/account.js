export default {
    namespaced: true,
    state: {
        user: undefined,
        permissions: null,
        roles: null,
        routesConfig: null,
        loginConfig: null,
        systemAgile: []
    },
    getters: {
        user: state => {
            if (!state.user) {
                try {
                    const user = localStorage.getItem(process.env.VUE_APP_USER_KEY)
                    state.user = JSON.parse(user)
                } catch (e) {
                    console.error(e)
                }
            }
            return state.user
        },
        permissions: state => {
            if (!state.permissions) {
                try {
                    const permissions = localStorage.getItem(process.env.VUE_APP_PERMISSIONS_KEY)
                    state.permissions = JSON.parse(permissions)
                    state.permissions = state.permissions ? state.permissions : []
                } catch (e) {
                    console.error(e.message)
                }
            }
            return state.permissions
        },
        roles: state => {
            if (!state.roles) {
                try {
                    const roles = localStorage.getItem(process.env.VUE_APP_ROLES_KEY)
                    state.roles = JSON.parse(roles)
                    state.roles = state.roles ? state.roles : []
                } catch (e) {
                    console.error(e.message)
                }
            }
            return state.roles
        },
        routesConfig: state => {
            if (!state.routesConfig) {
                try {
                    const routesConfig = localStorage.getItem(process.env.VUE_APP_ROUTES_KEY)
                    state.routesConfig = JSON.parse(routesConfig)
                    state.routesConfig = state.routesConfig ? state.routesConfig : []
                } catch (e) {
                    console.error(e.message)
                }
            }
            return state.routesConfig
        },
        loginConfig: state => {
            if (!state.loginConfig) {
                try {
                    const loginConfig = localStorage.getItem(process.env.VUE_APP_LOGINCONFIG_KEY)
                    state.loginConfig = JSON.parse(loginConfig)
                    state.loginConfig = state.loginConfig ? state.loginConfig : []
                } catch (e) {
                    console.error(e.message)
                }
            }
            return state.loginConfig
        },
        systemAgile: state => {
            return state.systemAgile
        }
    },
    mutations: {
        setUser(state, user) {
            state.user = user
            localStorage.setItem(process.env.VUE_APP_USER_KEY, JSON.stringify(user))
        },
        setPermissions(state, permissions) {
            state.permissions = permissions
            localStorage.setItem(process.env.VUE_APP_PERMISSIONS_KEY, JSON.stringify(permissions))
        },
        setRoles(state, roles) {
            state.roles = roles
            localStorage.setItem(process.env.VUE_APP_ROLES_KEY, JSON.stringify(roles))
        },
        setRoutesConfig(state, routesConfig) {
            state.routesConfig = routesConfig
            localStorage.setItem(process.env.VUE_APP_ROUTES_KEY, JSON.stringify(routesConfig))
        },
        setLoginConfig(state, loginConfig) {
            state.loginConfig = loginConfig
            localStorage.setItem(process.env.VUE_APP_LOGINCONFIG_KEY, JSON.stringify(loginConfig))
        },
        setSystemAgile(state, systemAgile) {
            state.systemAgile = systemAgile
        }
    }
}