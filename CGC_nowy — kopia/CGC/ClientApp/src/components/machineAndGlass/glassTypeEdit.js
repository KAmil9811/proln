import React, { Component } from 'react';
import './glassTypeEdit.css'
import Sidebar from '../Sidebar';

export class GlassTypeEdit extends Component {
    displayName = GlassTypeEdit.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
        }
    }
    changeType = (event) => {
        event.preventDefault();
        const receiver = {
            user: {
                login: sessionStorage.getItem('login')
            },
            new_type: this.refs.type.value,
            old_type: sessionStorage.getItem('type')
        }
        fetch(`api/Magazine/Change_type_Admin`, {
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
                if (json[0] === 'New_Type_already_exist') {
                    alert("Taki typ już istnieje")
                }
                else {
                    alert("Typ został zedytowany")
                    this.props.history.push('/glassatibutes')
                    sessionStorage.removeItem('typ')
                }
            })
    }

    return = (event) => {
        event.preventDefault();
        sessionStorage.removeItem('type')
        this.props.history.push('/glassatibutes')
    }

    render() {
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Zaloguj się, aby usyskać dostęp!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Logowanie</button>
                </div>
            );
        }
        else {
            return (
                <div className="typeedit">
                    <Sidebar />
                    <div className="typeEdit">
                        <form>
                            <div className="form-group">
                                <h2>Edytuj typ:</h2>
                                <input
                                    type="text"
                                    name="color"
                                    className="form-control"
                                    id="inputColor"
                                    placeholder="Podaj typ"
                                    ref="type"
                                    defaultValue={sessionStorage.getItem('type')}
                                />
                            </div>

                            <button type="button" className="danger_type_edit" onClick={this.return}>Anuluj</button>
                            <button type="button" className="success_type_edit" onClick={this.changeColor}>Edytuj</button>
                        </form>
                    </div>
                </div>
            )
        }
    }
}