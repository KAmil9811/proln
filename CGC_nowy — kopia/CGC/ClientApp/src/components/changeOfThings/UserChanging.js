import React, { Component } from "react";
import './UserChanging.css';

export class UserChanging extends Component {
    displayName = UserChanging.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            magazineManagement: false,
            machineManagement: false,
            orderManagement: false,
            cutManagement: false,
        }
    }

    handleUserChanging = (event) => {
        event.preventDefault();
        const receiver = {
            admin: {
                login: sessionStorage.getItem('login')
            },
            user: {
                login: sessionStorage.getItem('editLogin'),
                password: this.refs.password.value,
                email: this.refs.email.value,
                name: this.refs.name.value,
                surname: this.refs.surname.value,
                Magazine_management: this.state.magazineManagement,
                Machine_management: this.state.machineManagement,
                Order_management: this.state.orderManagement,
                Cut_management: this.state.cutManagement,
            },
            perm: this.state.value,
        }
        
        fetch(`api/Users/Edit_User_Admin`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json)
                return (json);
            })
            .then(json => {
                sessionStorage.removeItem('editPerm');
                sessionStorage.removeItem('editLogin');
                sessionStorage.removeItem('editPassword');
                sessionStorage.removeItem('editName');
                sessionStorage.removeItem('editSecondName');
                this.props.history.push('/controlpanel');
            })   
                
            

        
    }

    cancelUserChanging = (event) => {
        this.props.history.push('/controlpanel');
        sessionStorage.removeItem('editPerm');
        sessionStorage.removeItem('editLogin');
        sessionStorage.removeItem('editPassword');
        sessionStorage.removeItem('editName');
        sessionStorage.removeItem('editSecondName');
    }
    changeUser = (event) => {
        this.props.history.push('/controlpanel');
        sessionStorage.removeItem('editPerm');
        sessionStorage.removeItem('editLogin');
        sessionStorage.removeItem('editPassword');
        sessionStorage.removeItem('editName');
        sessionStorage.removeItem('editSecondName');
    }

    permRender() {
        if (sessionStorage.getItem('manager') === 'true') {
            return (
                <div className="form-group">
                    <label>Uprawnienia:</label><br />
                    <select onChange={(e) => {
                        this.setState({ value: e.target.value });
                        console.log(this.state)
                    }} >
                        <option value="user">Pracownik</option>
                        <option value="admin">Admin</option>
                        <option value="superAdmin">Super admin</option>
                    </select><br />
                    <input type="checkbox" onChange={(e) => this.setState({ magazineManagement: !this.state.magazineManagement })} />   Magazynier<br />
                    <input type="checkbox" onChange={(e) => this.setState({ machineManagement: !this.state.machineManagement })} />   Maszynista<br />
                    <input type="checkbox" onChange={(e) => this.setState({ orderManagement: !this.state.orderManagement })} />   Menedżer zamówień<br />
                    <input type="checkbox" onChange={(e) => this.setState({ cutManagement: !this.state.cutManagement })} />   Menedżer cięcia<br />
                </div>
            )
        }
        else if (sessionStorage.getItem('superAdmin') === 'true') {
            return (
                <div className="form-group">
                    <label>Uprawnienia:</label><br />
                    <select onChange={(e) => {
                        this.setState({ value: e.target.value });
                        console.log(this.state)
                    }} >
                        <option value="user">Pracownik</option>
                        <option value="admin">Admin</option>
                    </select><br />
                    <input type="checkbox" onChange={(e) => this.setState({ magazineManagement: !this.state.magazineManagement })} />   Magazynier<br />
                    <input type="checkbox" onChange={(e) => this.setState({ machineManagement: !this.state.machineManagement })} />   Maszynista<br />
                    <input type="checkbox" onChange={(e) => this.setState({ orderManagement: !this.state.orderManagement })} />   Menedżer zamówień<br />
                    <input type="checkbox" onChange={(e) => this.setState({ cutManagement: !this.state.cutManagement })} />   Menedżer cięcia<br />
                </div>
            )
        }
        else {
            return (
                <div className="form-group">
                    <label>Uprawnienia:</label><br />
                    <select onChange={(e) => {
                        this.setState({ value: e.target.value });
                        console.log(this.state)
                    }} >
                        <option value="user">Pracownik</option>
                    </select><br />
                    <input type="checkbox" onChange={(e) => this.setState({ magazineManagement: !this.state.magazineManagement })} />   Magazynier<br />
                    <input type="checkbox" onChange={(e) => this.setState({ machineManagement: !this.state.machineManagement })} />   Maszynista<br />
                    <input type="checkbox" onChange={(e) => this.setState({ orderManagement: !this.state.orderManagement })} />   Menedżer zamówień<br />
                    <input type="checkbox" onChange={(e) => this.setState({ cutManagement: !this.state.cutManagement })} />   Menedżer cięcia<br />
                </div>
            )
        }
    }


    render() {
        var perm = this.permRender();
        return (
            <div className="userChange">
                <form>
                    <div className="form-group">
                        <h2>Edycja konta</h2>
                        <label>Login: {sessionStorage.getItem('editLogin')}</label>
                    </div>
                    <div className="form-group">
                        <label>Email</label>
                        <input
                            type="email"
                            name="text"
                            className="form-control"
                            id="inputEmail"
                            defaultValue={sessionStorage.getItem('editMail')}
                            ref="email"
                        />
                    </div>
                    <div className="form-group">
                        <label>Hasło</label>
                        <input
                            type="text"
                            className="form-control"
                            id="inputPassword"
                            defaultValue={sessionStorage.getItem('editPassword')}
                            ref="password"
                        />
                    </div>
                    <div className="form-group">
                        <label>Imie</label>
                        <input
                            type="text"
                            className="form-control"
                            id="inputFirstName"
                            defaultValue={sessionStorage.getItem('editName')}
                            ref="name"
                        />
                    </div>
                    <div className="form-group">
                        <label>Nazwisko</label>
                        <input
                            type="text"
                            className="form-control"
                            id="inputSecondName"
                            defaultValue={sessionStorage.getItem('editSecondName')}
                            ref="surname"
                        />
                    </div>
                    <div className="form-group">
                        <label>Aktualne uprawnienia: {sessionStorage.getItem('editPerm')}</label>
                        
                    </div>
                    <div className="form-group">
                        <label>Jakie uprawnienia po edycji ma posiadać pracownik?</label>
                        {perm}

                    </div>
                    
                    <div className="form-group">
                        <button type="submit" className="cancel_u" onClick={this.cancelUserChanging}>Anuluj</button>
                        <button type="submit" className="apply_changes" onClick={this.handleUserChanging}>Zastosuj zmiany</button>
                        
                    </div>

                </form>
            </div>
        );
    }
}