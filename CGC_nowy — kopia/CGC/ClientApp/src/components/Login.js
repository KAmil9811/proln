import React, { Component, useState } from 'react';
import './Login.css'


export class Login extends Component {
    displayName = Login.name;
    constructor(props) {
        super(props);
    }


    resetPassword1 = (event) => {
        this.props.history.push('/reset_password1')
    }

    handleLoging = (event) => {
        event.preventDefault();
        const user = {
            login: this.refs.login.value,
            password: this.refs.password.value
        }

        //Przekazujesz tu całego user
        fetch(`api/Users/Log_in`, {
            method: "post",
            body: JSON.stringify(
                user
            ),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            /*.then(console.log(user))
            .then(console.log(JSON.stringify(user)))*/
            .then(console.log(HTMLBodyElement))
            .then(console.log(user))
            .then(res => res.json())
            .then(json => {
                console.log(json)
                return(json)
            })
            .then(json => {
                const access2 = json[0].error_Messege;
                /*console.log(access2)*/
                if (access2 == null) {
                    sessionStorage.setItem('email', json[0].email);
                    sessionStorage.setItem('login', json[0].login);
                    sessionStorage.setItem('name', json[0].name);
                    sessionStorage.setItem('password', json[0].password);
                    sessionStorage.setItem('surname', json[0].surname);
                    // permissions
                    sessionStorage.setItem('admin', json[0].admin);
                    sessionStorage.setItem('superAdmin', json[0].super_Admin);
                    sessionStorage.setItem('manager', json[0].manager);
                    sessionStorage.setItem('magazineManagement', json[0].magazine_management);
                    sessionStorage.setItem('machineManagement', json[0].machine_management);
                    sessionStorage.setItem('orderManagement', json[0].order_management);
                    sessionStorage.setItem('cutManagement', json[0].cut_management);
                    this.props.history.push('/home');
                }
                else {
                    alert("Zły login lub hasło!");
                    /*console.log('wlogin= ' + user.login);
                    console.log('whasło= ' + user.password);
                    console.log(user);
                    console.log('login który dostaje '  + json[0].login);*/
                }
            })     
    }
   
    render() {
        return (
            <div className="Login">
                <form>
                    <div className="form-group">
                        <label>Login</label>
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
                        <label>Hasło</label>
                        <input
                            type="password"
                            className="form-control"
                            id="inputPassword"
                            placeholder="Hasło"
                            ref="password"  
                        />
                    </div>
                    <button type="submit" className="log_in" onClick={this.handleLoging} >Zaloguj</button>
                    <button className="reset_password" onClick={this.resetPassword1} >Zresetuj hasło</button>
                   </form>
            </div>
        );
    }
}
