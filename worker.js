export default {
    async fetch(request) {
        const url = new URL(request.url);

        if (url.pathname === '/api') {
            const ip = request.headers.get('CF-Connecting-IP') || 'Unknown';
            const isIPv6 = ip.includes(':');
            const protocol = isIPv6 ? 'IPv6' : 'IPv4';

            const headers = {
                'Cache-Control': 'no-store',
                'X-Content-Type-Options': 'nosniff',
            };

            const format = url.searchParams.get('format');

            if (format === 'json') {
                return new Response(
                    JSON.stringify({ ip, protocol }),
                    { headers: { ...headers, 'Content-Type': 'application/json' } }
                );
            }

            return new Response(ip, {
                headers: { ...headers, 'Content-Type': 'text/plain' },
            });
        }

        return new Response('Not Found', { status: 404 });
    }
}
