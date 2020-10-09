import Link from 'next/link'

export default function Navbar() {
    return(
        <nav className="navbar has-shadow" role="navigation" aria-label="main navigation">
            <div className="container">
                <div className="navbar-brand">
                    <div className="navbar-item">
                        <Link href="/" >
                            <img src="/modelsaber-logo-web.svg" alt="ModelSaber Logo" width="24" height="24" />
                        </Link>
                    </div>
                    <a role="button" className="navbar-burger burger" aria-label="menu" aria-expanded="false">
                        <span aria-hidden="true" />
                        <span aria-hidden="true" />
                        <span aria-hidden="true" />
                    </a>
                </div>
                <div className="navbar-menu">
                    <div className="navbar-start">
                        <Link href="/">
                            <a className="navbar-item">
                                Home
                            </a>
                        </Link>
                        <Link href="/about">
                            <a className="navbar-item">
                                About
                            </a>
                        </Link>
                        <a className="navbar-item has-dropdown is-hoverable">
                            <a className="navbar-link">
                                Resources
                            </a>
                            <div className="navbar-dropdown">
                                <a className="navbar-item">
                                    Repository
                                </a>
                                <a className="navbar-item">
                                    API
                                </a>
                            </div>
                        </a>
                        <a className="navbar-item has-dropdown is-hoverable">
                            <a className="navbar-link">
                                Extra Links
                            </a>
                            <div className="navbar-dropdown">
                                <a className="navbar-item">
                                    Donate (Assistant)
                                </a>
                                <a className="navbar-item">
                                    Donate (Auros)
                                </a>
                                <a className="navbar-item">
                                    BSMG
                                </a>
                            </div>
                        </a>
                    </div>
                    <div className="navbar-end">
                        <div className="navbar-item">
                            <div className="buttons">
                                <Link href="/login">
                                    <a className="button">
                                        Log In
                                    </a>
                                </Link>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </nav>
    )
}