import React, { Component, useState } from 'react';
import './Login.css'


export class Login extends Component {
    displayName = Login.name;
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        sessionStorage.removeItem('email')
        sessionStorage.removeItem('login')
        sessionStorage.removeItem('name')
        sessionStorage.removeItem('password')
        sessionStorage.removeItem('surname')
        sessionStorage.removeItem('user')
        sessionStorage.removeItem('admin')
        sessionStorage.removeItem('superAdmin')
        sessionStorage.removeItem('manager')
        sessionStorage.removeItem('magazineManagement')
        sessionStorage.removeItem('machineManagement')
        sessionStorage.removeItem('orderManagement')
        sessionStorage.removeItem('cutManagement')
        sessionStorage.setItem('valid', '')
        var title = 'Home'
        sessionStorage.setItem('title', title)
        sessionStorage.setItem('sidebar', false);
        sessionStorage.setItem('sidebar2', true);
    }

    resetPassword1 = (event) => {
        this.props.history.push('/reset_password1')
    }

    handleLoging = (event) => {
        event.preventDefault();
        const model = {
            login: this.refs.login.value,
            password: this.refs.password.value,
            company: this.refs.firm.value
            
        }

        //Przekazujesz tu całego user
        fetch(`Api/Values/authenticate`, {
            method: "post",
            body: JSON.stringify(
               model
            ),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            /*.then(console.log(user))
            .then(console.log(JSON.stringify(user)))*/
         
            .then(res => res.json())
            .then(json => {
                return(json)
            })
           .then(json => {
               const access2 = json.message;
               const access = json.status;
                /*console.log(access2)*/
               if (access2 == "Username or password is incorrect" || access == 400 ) {
                   alert("Wrong login or password!");
               }
               else {
                    sessionStorage.setItem('token', json.token);
                   sessionStorage.setItem('email', json.email);

                   sessionStorage.setItem('login', json.login);
                   sessionStorage.setItem('company', this.refs.firm.value);
                    sessionStorage.setItem('name', json.name);
                    sessionStorage.setItem('surname', json.surname);
                    // permissions
                    sessionStorage.setItem('admin', json.admin);
                    sessionStorage.setItem('superAdmin', json.super_Admin);
                    sessionStorage.setItem('manager', json.manager);
                    sessionStorage.setItem('magazineManagement', json.magazine_management);
                    sessionStorage.setItem('machineManagement', json.machine_management);
                    sessionStorage.setItem('orderManagement', json.order_management);
                    sessionStorage.setItem('cutManagement', json.cut_management);
                    sessionStorage.setItem('valid', '1');
                    this.props.history.push('/home');
                }
           })     
    }

   
    render() {
        return (

            <div className="Login">




                <div className="tit_log">

                    <h1 className="tit_text">Login</h1>
                </div>
                <form>
                    <div className="Login_c">
                        <div className="form-group">
                            <label>Firm:</label>
                            <input
                                type="text"
                                name="Firm"
                                className="form-control"
                                id="inputFirm"
                                placeholder="Firm"
                                ref="firm"
                            />
                        </div>
                        <div className="form-group">
                            <label>Login:</label>
                            <input
                                type="text"
                                name="Login"
                                className="form-control"
                                id="inputLogin"
                                placeholder="Login"
                                ref="login"
                            />
                        </div>
                        <div className="form-group">
                            <label>Password:</label>
                            <input
                                type="password"
                                className="form-control"
                                id="inputPassword"
                                placeholder="*********"
                                ref="password"
                            />
                        </div>
                        <div className="buttonLogin">
                            <button type="submit" className="successs_login" onClick={this.handleLoging} >Log in</button>
                            <button className="danger_resset_pass" onClick={this.resetPassword1} >Reset password</button>


                        </div>
                    </div>
                   
                </form>
            </div>

        );
    }
}
