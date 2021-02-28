import React, { Component } from "react";
import './UserChanging.css';
import Sidebar from '../Sidebar';

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
                'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
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
                this.props.history.push('/controlpaneladmin');
            })   
                
            

        
    }
    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }

    cancelUserChanging = (event) => {
        this.props.history.push('/controlpaneladmin');
        sessionStorage.removeItem('editPerm');
        sessionStorage.removeItem('editLogin');
        sessionStorage.removeItem('editPassword');
        sessionStorage.removeItem('editName');
        sessionStorage.removeItem('editSecondName');
    }
    changeUser = (event) => {
        this.props.history.push('/controlpaneladmin');
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
                    <label>Permission:</label><br />
                    <select onChange={(e) => {
                        this.setState({ value: e.target.value });
                        console.log(this.state)
                    }} >
                        <option value="user">Employee</option>
                        <option value="admin">Admin</option>
                        <option value="superAdmin">Super admin</option>
                    </select><br />
                    <input type="checkbox" onChange={(e) => this.setState({ magazineManagement: !this.state.magazineManagement })} />   Magazine management<br />
                    <input type="checkbox" onChange={(e) => this.setState({ machineManagement: !this.state.machineManagement })} />   Machine management<br />
                    <input type="checkbox" onChange={(e) => this.setState({ orderManagement: !this.state.orderManagement })} />   Order management<br />
                    <input type="checkbox" onChange={(e) => this.setState({ cutManagement: !this.state.cutManagement })} />   Cut management<br />
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
                        <option value="user">Employee</option>
                        <option value="admin">Admin</option>
                    </select><br />
                    <input type="checkbox" onChange={(e) => this.setState({ magazineManagement: !this.state.magazineManagement })} />   Magazine management<br />
                    <input type="checkbox" onChange={(e) => this.setState({ machineManagement: !this.state.machineManagement })} />   Machine management<br />
                    <input type="checkbox" onChange={(e) => this.setState({ orderManagement: !this.state.orderManagement })} />   Order management<br />
                    <input type="checkbox" onChange={(e) => this.setState({ cutManagement: !this.state.cutManagement })} />   Cut management<br />
                </div>
            )
        }
        else {
            return (
                <div className="form-group">
                    <label>Permission:</label><br />
                    <select onChange={(e) => {
                        this.setState({ value: e.target.value });
                        console.log(this.state)
                    }} >
                        <option value="user">Employee</option>
                    </select><br />
                    <input type="checkbox" onChange={(e) => this.setState({ magazineManagement: !this.state.magazineManagement })} />   Magazine management<br />
                    <input type="checkbox" onChange={(e) => this.setState({ machineManagement: !this.state.machineManagement })} />   Machine management<br />
                    <input type="checkbox" onChange={(e) => this.setState({ orderManagement: !this.state.orderManagement })} />   Order management<br />
                    <input type="checkbox" onChange={(e) => this.setState({ cutManagement: !this.state.cutManagement })} />   Cut management<br />
                </div>
            )
        }
    }


    render() {
        var perm = this.permRender();
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if ( sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true'){
            return (
                <div className="UserChange">
                    <div>
                        <Sidebar />
                        <div className="title">
                            <h1 className="titletext">Change user</h1>
                        </div>
                    </div>
                    <form>
                        <div className="UserChange_c">

                            <div className="form-group">

                                <label>Login: {sessionStorage.getItem('editLogin')}</label>
                            </div>
                            <div className="form-group">
                                <label>Email:</label>
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
                                <label>Password:</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputPassword"
                                    defaultValue={sessionStorage.getItem('editPassword')}
                                    ref="password"
                                />
                            </div>
                            <div className="form-group">
                                <label>Name:</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputFirstName"
                                    defaultValue={sessionStorage.getItem('editName')}
                                    ref="name"
                                />
                            </div>
                            <div className="form-group">
                                <label>Surname:</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputSecondName"
                                    defaultValue={sessionStorage.getItem('editSecondName')}
                                    ref="surname"
                                />
                            </div>
                            <div className="form-group">
                                <label>Current permissions: {sessionStorage.getItem('editPerm')}</label>

                            </div>
                            <div className="form-group">
                                <label>What permission after editing should the employee have?</label>
                                {perm}

                            </div>

                            <div className="form-group">

                                <button type="submit" className="success_user_change" onClick={this.handleUserChanging}>Change user</button>

                                <button type="submit" className="danger_user_change" onClick={this.cancelUserChanging}>Cancel</button>

                            </div>


                        </div>
                    </form>
                </div>


                
                );
        }
        else {
            return (
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                </div>
            );

        }
    }
}