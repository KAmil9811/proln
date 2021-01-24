import React, { Component } from 'react';
import './glassColorEdit.css';
import Sidebar from '../Sidebar';

export class GlassColorEdit extends Component {
    displayName = GlassColorEdit.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
        }
    }
    changeColor = (event) => {
        event.preventDefault();
        const receiver = {
            user: {
                login: sessionStorage.getItem('login')
            },
            new_color: this.refs.color.value,
            old_color: sessionStorage.getItem('color')
        }
        fetch(`api/Magazine/Change_Color_Admin`, {
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
                if (json[0] === 'New_Color_already_exist') {
                    alert("Taki kolor ju¿ istnieje")
                }
                else {
                    alert("Kolor zosta³ zedytowany")
                    this.props.history.push('/glassatibutes')
                    sessionStorage.removeItem('color')
                }
                })
    }

    return = (event) => {
        event.preventDefault();
        sessionStorage.removeItem('color')
        this.props.history.push('/glassatibutes')
    }
    
    render() {
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Zaloguj siê, aby usyskaæ dostêp!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Logowanie</button>
                </div>
            );
        }
        else {
            return (
                <div className="editColor">
                    <Sidebar />
                    <div className="EditColor">
                        <form>
                            <div className="form-group">
                                <h2>Edytuj kolor:</h2>
                                <input
                                    type="text"
                                    name="color"
                                    className="form-control"
                                    id="inputColor"
                                    placeholder="Podaj kolor"
                                    ref="color"
                                    defaultValue={sessionStorage.getItem('color')}
                                />
                            </div>

                            <button type="button" className="danger_color_edit" onClick={this.return}>Anuluj</button>
                            <button type="button" className="success_color_edit" onClick={this.changeColor}>Edytuj</button>
                        </form>
                    </div>
                </div>
            )
        }
    }
}