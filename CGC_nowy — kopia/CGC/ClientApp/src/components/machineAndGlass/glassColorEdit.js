import React, { Component } from 'react';
import './glassColorEdit.css';

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
                    alert("Kolor zosta³ zedytowany")
                    this.props.history.push('/glassatibutes')
                    sessionStorage.removeItem('color')
                })
    }

    return = (event) => {
        event.preventDefault();
        sessionStorage.removeItem('color')
        this.props.history.push('/glassatibutes')
    }
    
    render() {
        return (
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
                
                    <button type="button" className="cancel_edit_gla_c" onClick={this.return}>Cofnij</button>
                    <button type="button" className="edit_glass_c" onClick={this.changeColor}>Edytuj</button>
                </form>
            </div>
            )
    }
}