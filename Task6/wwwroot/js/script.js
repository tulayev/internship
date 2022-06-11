document.addEventListener('DOMContentLoaded', () => {
    const table = document.getElementById('table')
    const tbody = table.querySelector('table tbody')
    const errorOccuranceRange = document.getElementById('errorOccuranceRange')
    const errorOccuranceValue = document.getElementById('errorOccuranceValue')
    const seedValue = document.getElementById('seedValue')
    const seedBtn = document.getElementById('seedBtn')

    let step = 20
    let users = []
    let seed = 0
    let counter = 0

    const loadMore = (start, end, users) => {
        for (let i = start; i < end; i++) {
            const item = `
                <tr class="border-b">
                    <td class="px-6 py-4 whitespace-nowrap  font-medium text-gray-900">
                        ${++counter}
                    </td>
                    <td class="text-gray-900 px-6 py-4 whitespace-nowrap">
                        ${users[i].uniqueId}
                    </td>
                    <td class="text-gray-900 px-6 py-4 whitespace-nowrap">
                        ${users[i].name}
                    </td>
                    <td class="text-gray-900 px-6 py-4 whitespace-nowrap">
                        ${users[i].address}
                    </td>
                    <td class="text-gray-900 px-6 py-4 whitespace-nowrap">
                        ${users[i].phoneNumber}
                    </td>
                </tr>
            `
            tbody.innerHTML += item
        }
    }

    const loadUsers = async (seed, occurance) => {
        const urlString = window.location.href
        const url = new URL(urlString)
        const localeParam = url.searchParams.get('culture')

        try {
            let res

            if (localeParam) {
                res = await fetch(`/api/users?seed=${seed}&culture=${localeParam}&occurance=${occurance}`)
            } else {
                res = await fetch(`/api/users?seed=${seed}&occurance=${occurance}`)
            }

            users = await res.json()

            if (users.length > 0) {
                step = 20
                loadMore(0, step, users)
            }
        } catch (e) {
            console.log(e.message)
        }
    }

    table.addEventListener('scroll', () => {
        if (table.scrollTop + table.clientHeight >= table.scrollHeight) {
            if (users.length > 0) {
                loadMore(step, step + 10, users)
                step += 10
            }
        }
    })

    errorOccuranceRange.addEventListener('change', (e) => {
        counter = 0
        errorOccuranceValue.value = e.target.value
        tbody.innerHTML = ''
        loadUsers(seed, errorOccuranceValue.value)
    })
    
    errorOccuranceValue.addEventListener('input', (e) => {
        counter = 0
        tbody.innerHTML = ''
        loadUsers(seed, e.target.value)
    })

    seedValue.addEventListener('keyup', (e) => {
        if (e.target.value.length === 0) {
            errorOccuranceRange.disabled = true
            errorOccuranceValue.disabled = true
            seedBtn.disabled = true
            if (seedBtn.classList.contains('cursor-pointer')) {
                seedBtn.classList.remove('cursor-pointer')
                seedBtn.classList.add('cursor-not-allowed')
            }
        } else {
            errorOccuranceRange.disabled = false
            errorOccuranceValue.disabled = false
            seedBtn.disabled = false
            if (seedBtn.classList.contains('cursor-not-allowed')) {
                seedBtn.classList.remove('cursor-not-allowed')
                seedBtn.classList.add('cursor-pointer')
            }
        }
    })

    seedBtn.addEventListener('click', () => {
        counter = 0
        tbody.innerHTML = ''
        seed = seedValue.value
        loadUsers(seed, 0)
    })
})