import React, { Component } from "react";
import Select from 'react-select';
import './AddAcount.css';
import Sidebar from '../Sidebar';


export class AddAcount extends Component{
    displayName = AddAcount.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            magazineManagement: false,
            machineManagement: false,
            orderManagement: false,
            cutManagement: false,
            user:false,
        }
    }



    handleAddAcount = (event) => {
        event.preventDefault();
        if (this.state.value === "user") {
            this.setState({ user: true})
        }
        const receiver = {
            admin: {
                login: sessionStorage.getItem('login')
            },
            user: {
                login: this.refs.login.value,
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
        const admin = {
            login: sessionStorage.getItem('login')
        }
        console.log(receiver);
        console.log(admin);
        fetch(`api/Users/Add_User_Admin`, {
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
                const access2 = json[0].error_Messege
                console.log(access2)
                if (access2 == null) {
                    console.log("Dodano użytkownika")
                    this.props.history.push('/controlpanel')
                }
                else if (access2 == "Wrong_passowrd") {
                    alert("Hasło musi zawierać dużą i małą literę oraz cyfrę i zawierać min 8 znaków!")
                }
                else if (access2 == "Wrong_login") {
                    alert("Login moze zawierać jedynie znaki angielskiego alfabetu!")
                }
                else if (access2 == "Login_taken") {
                    alert("Login zajęty!")
                }
                else if (access2 == "Email_taken") {
                    alert("Email zajęty!")
                }
                else if (access2 == "Wrong_email") {
                    alert("Email niepoprawny!")
                }
                else {
                    alert("Coś poszło nie tak :(")
                }
            })
    }

    cancelAdding = (event) => {
        this.props.history.push('/controlpanel')
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
                        <div className="uprawnienia">
                        <input className="check" type="checkbox" onChange={(e) => this.setState({ magazineManagement: !this.state.magazineManagement })} />   Magazynier<br />
                        <input className="check" type="checkbox" onChange={(e) => this.setState({ machineManagement: !this.state.machineManagement })} />  Menedżer maszyn<br />
                        <input className="check" type="checkbox" onChange={(e) => this.setState({ orderManagement: !this.state.orderManagement })} />   Menedżer zamówień<br />
                        <input className="check" type="checkbox" onChange={(e) => this.setState({ cutManagement: !this.state.cutManagement })} />   Menedżer cięcia<br />
                        </div>
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
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ magazineManagement: !this.state.magazineManagement })} />   Magazynier<br />
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ machineManagement: !this.state.machineManagement })} />   Menedżer maszyn<br />
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ orderManagement: !this.state.orderManagement })} />   Menedżer zamówień<br />
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ cutManagement: !this.state.cutManagement })} />   Menedżer cięcia<br />
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
                    </select><br/>
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ magazineManagement: !this.state.magazineManagement })} />   Magazynier<br />
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ machineManagement: !this.state.machineManagement })} />   Menedżer maszyn<br />
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ orderManagement: !this.state.orderManagement })} />   Menedżer zamówień<br />
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ cutManagement: !this.state.cutManagement })} />   Menedżer cięcia<br />
                </div>
            )
        }
    }
    

    render() {
        var perm = this.permRender()
        return (
            <div className="login">
                <Sidebar />
                <div className="Login">
                    <form>
                        <div className="form-group">
                            <h2>Dodawanie konta</h2>
                            <label>Email</label>
                            <input
                                type="email"
                                name="Email"
                                className="form-control"
                                id="inputEmail"
                                placeholder="Podaj email"
                                ref="email"
                            />
                        </div>
                        <div className="form-group">
                            <label>Login</label>
                            <input
                                type="text"
                                className="form-control"
                                id="inputLogin"
                                placeholder="Podaj login"
                                ref="login"
                            />
                        </div>
                        <div className="form-group">
                            <label>Hasło</label>
                            <input
                                type="password"
                                className="form-control"
                                id="inputPassword"
                                placeholder="Podaj hasło"
                                ref="password"   
                            />
                        </div>
                        <div className="form-group">
                            <label>Imie</label>
                            <input
                                type="text"
                                className="form-control"
                                id="inputFirstName"
                                placeholder="Podaj imie"
                                ref="name"
                            />
                        </div>
                        <div className="form-group">
                            <label>Nazwisko</label>
                            <input
                                type="text"
                                className="form-control"
                                id="inputSecondName"
                                placeholder="Podaj nazwisko"
                                ref="surname"
                            />
                        </div>
                        {perm}
                    
                        <div className="form-group">
                            <button type="button" className="cancel" onClick={this.cancelAdding}>Anuluj</button>
                            <button type="button" className="add_user2" onClick={this.handleAddAcount}>Dadaj użytkownika</button>
                       
                        </div>

                    </form>
                </div>
            </div>
        );
    }
}